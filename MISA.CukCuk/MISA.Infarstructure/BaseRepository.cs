using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
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
            //1. Tạo kết nối và truy vấn
            var customer = _dbConnection.Query<TEntity>("Proc_GetCustomerById", new { entityId = customerId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //2. Trả về dữ liệu
            return customer;
        }

        public TEntity GetEntityByCode(string entityCode)
        {
           
        }

        public int Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

       

        public int Insert(TEntity enitity)
        {
            throw new NotImplementedException();
        }

        public int Update(Guid entityId, TEntity entity)
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
