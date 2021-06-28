using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MISA.ApplicationCore.Interfaces
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region Declare
        IConfiguration _configuration;
        IDbConnection _dbConnection = null;
        string _connectionString = string.Empty;
        string _tableName;
        #endregion

        #region Constructer
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }
        #endregion

        #region Methods
        public IEnumerable<TEntity> GetEntities()
        {
            //1. Tạo kết nối và truy vấn
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure).ToList();

            //2. Trả về dữ liệu
            return entities;
        }
        public TEntity GetEntityById(Guid entityId)
        {
            //1. Lấy tên của khóa chính
            var keyName = GetKeyProperty().Name;

            //2. Tạo kết nối và truy vấn
            var entity = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}ById", new { employeeId = entityId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            
            //3. Trả về dữ liệu
            return entity;
        }

        //public TEntity GetEntityByCode(string entityCode)
        //{
        //    //1. Tạo kết nối và truy vấn
        //    var entity = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}ById", new { customerId = entityCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();

        //    //2. Trả về dữ liệu
        //    return entity;
        //}

        public int Delete(Guid entityId)
        {
            //1. Kết nối tới CSDL:
            int rowAffects = _dbConnection.Execute($"Proc_Delete{_tableName}ById", param: new { CustomerId = entityId }, commandType: CommandType.StoredProcedure);

            //2. Trả về số bản ghi bị ảnh hưởng
            return rowAffects;
        }



        public int Insert(TEntity entity)
        {
            //1. Duyệt các thuộc tính trên bản ghi và tạo parameters
            var parameters = MappingDbType(entity);

            //2. Kết nối tới CSDL:
            var rowAffects = _dbConnection.Execute($"Proc_Insert{entity}", parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int Update(Guid entityId, TEntity entity)
        {
            //customer.CustomerId = customerId;

            //1. Duyệt các thuộc tính trên customer và tạo parameters
            var parameters = MappingDbType(entity);

            //2. Kết nối tới CSDL:
            int rowAffects = _dbConnection.Execute($"Proc_Update{entity}", param: parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        private DynamicParameters MappingDbType(TEntity entity)
        {
            //1. Duyệt các thuộc tính trên entity và tạo parameters
            var properties = entity.GetType().GetProperties();

            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;

                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                else
                    parameters.Add($"@{propertyName}", propertyValue);
            }

            //2. Trả về danh sách các parameter
            return parameters;
        }
       
        private PropertyInfo GetKeyProperty()
        {
            var keyProperty = typeof(TEntity)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(KeyAttribute), false))
                .FirstOrDefault();
            
            return keyProperty;
        }
        #endregion
    }
}
