﻿using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        #region Declare
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult = null;
        List<string> _errors = null;
        #endregion

        #region Constructer
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            
            _serviceResult = new ServiceResult();

            if (_errors == null)
                _errors = new List<string>();
        }
        #endregion

        #region Methods
        public IEnumerable<TEntity> GetEntities()
        {
            var entities = _baseRepository.GetEntities();
            return entities;
        }

        public TEntity GetEntityById(Guid entityId)
        {
            var entity = _baseRepository.GetEntityById(entityId);
            return entity;
        }

        public virtual ServiceResult Insert(TEntity entity)
        {
            var isValid = Validate(entity);

            if (isValid)
            {
                _serviceResult.Data = _baseRepository.Insert(entity);
                _serviceResult.MISACode = MISACode.Valid;
                _errors.Clear();
                _errors.Add("Hợp lệ");
            }
            else
            {
                _serviceResult.MISACode = MISACode.InValid;
                _serviceResult.Messasge = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại";
            }

            return _serviceResult;
        }

        public ServiceResult Update(Guid entityId, TEntity entity)
        {
            var serviceResult = new ServiceResult();
            int rowAffects = _baseRepository.Update(entityId, entity);
            return serviceResult;
        }

        public ServiceResult Delete(Guid entityId)
        {
            var serviceResult = new ServiceResult();
            int rowAffects = _baseRepository.Delete(entityId);
            return serviceResult;
        }

        private bool Validate(TEntity entity, List<TEntity> resources = null)
        {
            var isValid = true;

            //1. Đọc các property
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);

                //1.1 Kiểm tra xem  có attribute cần phải validate không
                if (property.IsDefined(typeof(IRequired), false))
                {
                    //1.1.1 Check bắt buộc nhập
                    if (propertyValue == null)
                        isValid = validateRequired(propertyName, propertyValue, resources);
                }

                if (property.IsDefined(typeof(CheckDuplicate), false))
                {
                    //1.1.2 Check trùng
                    isValid = validateDuplicate(entity, propertyName, propertyValue, resources);
                }
            }

            return isValid;
        }

        private bool validateRequired(string propertyName, object propertyValue, List<TEntity> rources = null)
        {
            bool isValid = true;

            if (propertyValue == null)
            {
                isValid = false;

                var columnDisplayName = getAttributeDisplayName(propertyName);

                _serviceResult.MISACode = MISACode.InValid;
                _serviceResult.Messasge = "Có lỗi xảy ra, vui lòng kiểm tra lại.";
                _serviceResult.Data = new { devMsg = "Dữ liệu không được trống", userMsg = "Dữ liệu không được trống" };
            }

            return isValid;
        }

        private bool validateDuplicate(TEntity entity, string propertyName, object propertyValue, List<TEntity> rources = null)
        {
            bool isValid = true;

            var entityDuplicate = _baseRepository.GetEntityByProperty(propertyName, propertyValue);
            if (entityDuplicate != null)
            {
                isValid = false;

                var columnDisplayName = getAttributeDisplayName(propertyName);

                _serviceResult.MISACode = MISACode.InValid;
                _serviceResult.Messasge = "Có lỗi xảy ra, vui lòng kiểm tra lại.";
                _serviceResult.Data = new { devMsg = $"{columnDisplayName} bị trùng", userMsg = $"{columnDisplayName} bị trùng" };
            }

            return isValid;
        }

        public async Task<ServiceResult> readExcelFile(IFormFile formFile, CancellationToken cancellationToken)
        {

            if (formFile == null || formFile.Length <= 0)
            {
                _serviceResult.Data = -1;
                _serviceResult.Messasge = "Không mở được file";
                _serviceResult.MISACode = MISACode.InValid;

                return _serviceResult;
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _serviceResult.Data = -1;
                _serviceResult.Messasge = "File không được hỗ trợ";
                _serviceResult.MISACode = MISACode.InValid;

                return _serviceResult;
            }

            var list = new List<TEntity>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    //Thông tin các thuộc tính trong object
                    var properties = typeof(TEntity).GetProperties()
                                    .Where(p => p.IsDefined(typeof(DisplayAttribute), false))
                                    .Select(p => new
                                    {
                                        PropertyName = p.Name,
                                        DisplayName = p.GetCustomAttributes(typeof(DisplayAttribute),
                                                false).Cast<DisplayAttribute>().Single().Name,
                                        DataType = p.PropertyType
                                    });

                    for (var row = 3; row <= rowCount; row++)
                    {
                        var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });

                        for (var col = 1; col <= colCount; col++)
                        {
                            //Tên hiển thị của cột
                            string columnName = worksheet.Cells[2, col].Value != null
                                ? worksheet.Cells[2, col].Value.ToString().Trim()
                                : "";

                            string value = worksheet.Cells[row, col].Value != null
                                ? worksheet.Cells[row, col].Value.ToString().Trim()
                                : "";

                            //Tồn tại tên cột trong object
                            var hasColumnName = properties.FirstOrDefault(x => columnName.ToLower().Contains(x.DisplayName.ToLower()));
                            if (hasColumnName != null)
                            {
                                //Chuyển data vào object
                                dynamic temp = convertValue(hasColumnName.DataType, value);
                                entity.GetType().GetProperty(hasColumnName.PropertyName).SetValue(entity, temp);
                            }
                        }
                        list.Add(entity);
                    }
                }
            }

            //Validate
            if(list.Count > 0)
            {
                var allEntitiesFromDb = _baseRepository.GetEntities();
                foreach(var item in list)
                {
                    Validate(item, list);
                    Validate(item, allEntitiesFromDb.ToList());
                }
            }

            _serviceResult.Data = list;
            _serviceResult.Messasge = "OK";
            _serviceResult.MISACode = MISACode.Success;

            return _serviceResult;
        }

        /// <summary>
        /// Trả về dữ liệu với kiểu dữ liệu truyền vào
        /// </summary>
        /// <param name="type">Kiểu dữ liệu</param>
        /// <param name="value">Giá trị kiểu string</param>
        /// <returns></returns>
        private dynamic convertValue(Type type, string value)
        {
            dynamic res = null;
            if (value == "")
                return res;

            if (type.Name == "Nullable`1")
            {
                type = Nullable.GetUnderlyingType(type);

                if (type.Name == "DateTime")
                {
                    value = String.Join("-", value.Split('/').Reverse());
                }
            }

            res = Convert.ChangeType(value, type);

            return res;
        }

        /// <summary>
        /// Lấy tên hiển thị của trường trong entity
        /// </summary>
        /// <param name="attributeName">Tên thuộc tính</param>
        /// <returns>Tên hiển thị</returns>
        private String getAttributeDisplayName(string attributeName)
        {
            var res = typeof(TEntity).GetProperty(attributeName).GetCustomAttributes(typeof(DisplayAttribute),
                                                false).Cast<DisplayAttribute>().Single().Name;
            return res;
        }
        #endregion
    }
}