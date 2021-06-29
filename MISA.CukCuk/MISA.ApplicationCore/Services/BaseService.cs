using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        #endregion

        #region Constructer
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
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
            var serviceResult = new ServiceResult();

            var isValid = Validate(entity);

            if (isValid)
            {
                serviceResult.Data = _baseRepository.Insert(entity);
                serviceResult.MISACode = MISACode.Valid;
            }
            else
            {
                serviceResult.Data = null;
                serviceResult.MISACode = MISACode.InValid;
                serviceResult.Messasge = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại";
            }

            return serviceResult;
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

        private bool Validate(TEntity entity)
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
                        isValid = false;
                }

                if (property.IsDefined(typeof(CheckDuplicate), false))
                {
                    //1.1.2 Check trùng
                    var entityDuplicate = _baseRepository.GetEntityByProperty(propertyName, propertyValue);
                    if (entityDuplicate != null)
                        isValid = false;
                }
            }

            return isValid;
        }

        public async Task<ServiceResult> readExcelFile(IFormFile formFile, CancellationToken cancellationToken)
        {
            var serviceResult = new ServiceResult();

            if (formFile == null || formFile.Length <= 0)
            {
                serviceResult.Data = -1;
                serviceResult.Messasge = "Không mở được file";
                serviceResult.MISACode = MISACode.InValid;

                return serviceResult;
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                serviceResult.Data = -1;
                serviceResult.Messasge = "File không được hỗ trợ";
                serviceResult.MISACode = MISACode.InValid;

                return serviceResult;
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

                    var properties = typeof(TEntity).GetProperties()
                                    .Where(p => p.IsDefined(typeof(DisplayAttribute), false))
                                    .Select(p => new
                                    {
                                        PropertyName = p.Name,
                                        DisplayName = p.GetCustomAttributes(typeof(DisplayAttribute),
                                                false).Cast<DisplayAttribute>().Single().Name,
                                        DataType = p.GetType().Name
                                    });
                    //foreach(var column in properties)
                    //{
                    //    var attribute = column.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    //                             .Cast<DisplayNameAttribute>().Single();
                    //    string displayName = attribute.DisplayName;

                    //    for (var row = 2; row <= rowCount; row++)
                    //    {
                    //        list.Add(new TEntity
                    //        {
                    //            UserName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                    //            Age = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
                    //        });
                    //    }
                    //}

                    for (var row = 2; row <= rowCount; row++)
                    {
                        for (var col = 1; col <= colCount; col++)
                        {
                            //foreach (var property in properties)
                            //{
                            ////var propInfo = typeof(TEntity).GetProperty(property.Name);
                            //var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                            //var displayName = (displayNameAttribute[0] as DisplayNameAttribute).DisplayName;
                            ////var attribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                            ////               .Cast<DisplayNameAttribute>().Single();
                            ////string displayName = attribute.DisplayName;
                            //}
                            string test = worksheet.Cells[row, col].Value.ToString().Trim();
                        }
                    }
                }
            }

            serviceResult.Data = list;
            serviceResult.Messasge = "OK";
            serviceResult.MISACode = MISACode.Success;

            return serviceResult;
        }

        #endregion
    }
}
