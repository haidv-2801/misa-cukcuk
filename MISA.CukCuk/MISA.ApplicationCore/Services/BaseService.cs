using Microsoft.AspNetCore.Http;
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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult = null;
        IEnumerable<TEntity> _entityDbList = null;
        #endregion

        #region Constructer
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult()
            {
                MISACode = MISACode.Success
            };
            _entityDbList = new List<TEntity>();
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
            entity.EntityState = EntityState.Add;

            //1. Validate tất cả các trường nếu được gắn thẻ
            var isValid = Validate(entity);

            if (isValid)
            {
                _serviceResult.Data = _baseRepository.Insert(entity);
                _serviceResult.MISACode = MISACode.Valid;
                _serviceResult.Messasge = "Thêm thành công";
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
            entity.EntityState = EntityState.Update;

            //1. Validate tất cả các trường nếu được gắn thẻ
            var isValid = Validate(entity);
            if (isValid)
            {
                _serviceResult.Data = _baseRepository.Update(entityId, entity);
                _serviceResult.Messasge = "Sửa thành công";
            }
            else
            {
                _serviceResult.MISACode = MISACode.InValid;
                _serviceResult.Messasge = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại";
            }
            return _serviceResult;
        }

        public ServiceResult Delete(Guid entityId)
        {
            _serviceResult.Data = _baseRepository.Delete(entityId);
            return _serviceResult;
        }

        private bool Validate(TEntity entity)
        {
            var isValid = true;

            //1. Đọc các property
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                //1.1 Kiểm tra xem  có attribute cần phải validate không
                if (isValid && property.IsDefined(typeof(IRequired), false))
                {
                    //1.1.1 Check bắt buộc nhập
                    isValid = validateRequired(entity, property);
                }

                if (isValid && property.IsDefined(typeof(IDuplicate), false))
                {
                    //1.1.2 Check trùng
                    isValid = validateDuplicate(entity, property, entityDbSource);
                }
            }

            return isValid;
        }

        /// <summary>
        /// Validate dữ liệu import từ file
        /// </summary>
        /// <param name="entitiesImport">Danh sách thực thể cần validate</param>
        /// <returns>Kết quả validate (true-false)</returns>
        public bool validateDataImport(List<TEntity> entitiesImport)
        {
            _entityDbList = _baseRepository.GetEntities();
            bool allIsValid = true;
            IDictionary<object, List<string>> dict = new Dictionary<object, List<string>>();

            foreach (var entity in entitiesImport)
            {
                bool isValid = true;
                entity.Status = new List<string>();

                //1. Đọc các property
                var properties = entity.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(entity);
                    string displayName = getAttributeDisplayName(property.Name);

                    //1.1 Kiểm tra xem  có attribute cần phải validate không
                    if (property.IsDefined(typeof(IRequired), false))
                    {
                        //1.1.1 Check bắt buộc nhập
                        isValid = validateRequired(entity, property);
                        if (isValid == false)
                            entity.Status.Add($"{displayName} không được trống");
                    }

                    if (property.IsDefined(typeof(IDuplicate), false) && propertyValue != null)
                    {
                        //Check duplicate trên dãy data import
                        if (dict.ContainsKey(propertyValue) == true)
                        {
                            if (dict[propertyValue].Contains(property.Name))
                            {
                                isValid = false;
                                entity.Status.Add($"{displayName} {propertyValue} đã tồn tại");
                            }
                        }
                        else
                            dict.Add(propertyValue, new List<string>());

                        if (!dict[propertyValue].Contains(property.Name))
                            dict[propertyValue].Add(property.Name);

                        //Check duplicate trên server
                        isValid = validateDuplicate(entity, property, entityDbList) == true ? isValid : false;
                        if (isValid == false)
                            entity.Status.Add($"{displayName} {propertyValue} đã tồn tại trên nhập khẩu");
                    }
                }

                if (isValid == true)
                    entity.Status.Add("Hợp lệ");
                else
                    allIsValid = false;

            }

            return allIsValid;
        }

        private bool validateRequired(TEntity entity, PropertyInfo propertyInfo)
        {
            bool isValid = true;

            var propertyName = propertyInfo.Name;
            var propertyValue = propertyInfo.GetValue(entity);
            var propertyDisplayName = getAttributeDisplayName(propertyName);

            if (propertyValue == null)
            {
                isValid = false;

                _serviceResult.MISACode = MISACode.InValid;
                _serviceResult.Messasge = "Có lỗi xảy ra, vui lòng kiểm tra lại.";
                _serviceResult.Data = new { devMsg = $"{propertyDisplayName} không được trống", userMsg = $"{propertyDisplayName} không được trống" };
            }

            return isValid;
        }

        /// <summary>
        /// Validate duplicate
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="eSource">TEntity</param>
        /// <returns></returns>
        private bool validateDuplicate(TEntity entity, PropertyInfo propertyInfo, entitySource eSource)
        {
            bool isValid = true;

            var propertyName = propertyInfo.Name;
            var propertyDisplayName = getAttributeDisplayName(propertyName);

            //Tùy chỉnh nguồn dữ liệu để validate, trạng thái thêm hoắc sửa
            var entityDuplicate = eSource(entity, propertyInfo);

            if (entityDuplicate != null)
            {
                isValid = false;

                _serviceResult.MISACode = MISACode.InValid;
                _serviceResult.Messasge = "Có lỗi xảy ra, vui lòng kiểm tra lại.";
                _serviceResult.Data = new { devMsg = $"{propertyDisplayName} bị trùng", userMsg = $"{propertyDisplayName} bị trùng" };
            }

            return isValid;
        }

        /// <summary>
        /// Đọc file excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Danh sách các thực thể</returns>
        /// DVHAI (01/07/2021)
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

            //Lưu thông tin dãy các entity
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
            //Trên databse
            //Trên tệp import
            bool allIsValid = validateDataImport(list);
            _serviceResult.Data = list;

            if (allIsValid == true)
            {
                _serviceResult.Messasge = "OK";
                _serviceResult.MISACode = MISACode.Valid;
            }
            else
            {
                _serviceResult.Messasge = "Chưa OK";
                _serviceResult.MISACode = MISACode.InValid;
            }

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
            var res = attributeName;
            try
            {
                res = typeof(TEntity).GetProperty(attributeName).GetCustomAttributes(typeof(DisplayAttribute),
                                               false).Cast<DisplayAttribute>().Single().Name;
            }
            catch { }
            return res;
        }

        /// <summary>
        /// Điều chỉnh nguồn dữ liệu để validate
        /// </summary>
        /// <returns>TEntity</returns>
        /// DVHAI (07/01/2021)
        public delegate TEntity entitySource(TEntity entity, PropertyInfo propertyInfo);

        /// <summary>
        /// Nguồn enitty từ database để phục vụ validate theo trường hợp
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyInfo"></param>
        /// <returns>TEntity</returns>
        /// DVHAI (07/01/2021)
        private TEntity entityDbSource(TEntity entity, PropertyInfo propertyInfo)
        {

            var propertyName = propertyInfo.Name;
            var propertyValue = propertyInfo.GetValue(entity);

            //Mặc định là thêm
            var entityDuplicate = _baseRepository.GetEntityByProperty(propertyName, propertyValue);

            if (entity.EntityState == EntityState.Add)
                entityDuplicate = _baseRepository.GetEntityByProperty(propertyName, propertyValue);
            else if (entity.EntityState == EntityState.Update)
                entityDuplicate = _baseRepository.GetEntityByProperty(entity, propertyInfo);
            else
                entityDuplicate = null;

            return entityDuplicate;
        }

        /// <summary>
        /// Nguồn dữ liệu từ database phục vụ validate
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyInfo"></param>
        /// <returns>TEntity</returns>
        /// DVHAI (07/01/2021)
        private TEntity entityDbList(TEntity entity, PropertyInfo propertyInfo)
        {
            var propertyName = propertyInfo.Name;
            var propertyValue = propertyInfo.GetValue(entity);

            var item = _entityDbList.FirstOrDefault(x => x.GetType().GetProperty(propertyName).GetValue(x).ToString() == propertyValue.ToString());

            return item;
        }

        /// <summary>
        /// Cất nhiều bản ghi
        /// </summary>
        /// <param name="ieEntities"></param>
        /// <returns></returns>
        /// DVHAI (07/01/2021)
        public ServiceResult MultiInsert(IEnumerable<TEntity> ieEntities)
        {
            //Validate lại 1 lần nữa trên database
            int success = 0, fail = 0;
            
            bool allIsValid = validateDataImport(ieEntities.ToList());

            foreach(var entity in ieEntities)
            {
                if(entity.Status.Count() > 0 && entity.Status[0].Equals("Hợp lệ"))
                {
                    var rowAffect = _baseRepository.Insert(entity);
                    success += rowAffect;
                }
            }

            fail = ieEntities.Count() - success;

            _serviceResult.Data = new { successRecords = success, failRecords = fail };
            _serviceResult.Messasge = "";
            _serviceResult.MISACode = fail > 0 ? MISACode.InValid : MISACode.Success;

            return _serviceResult;
        }
        #endregion
    }
}
