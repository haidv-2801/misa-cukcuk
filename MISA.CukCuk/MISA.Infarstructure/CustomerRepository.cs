using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace MISA.Infarstructure
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
        #endregion

        #region Constructer
        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);

        }
        #endregion

        #region Methods
        public IEnumerable<Customer> GetCustomers()
        {
            //1. Tạo kết nối và truy vấn
            var customers = _dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure).ToList();

            //2. Trả về dữ liệu
            return customers;
        }

        public Customer GetCustomerByCode(string customerCode)
        {
            //1. Tạo kết nối và truy vấn
            var customer = _dbConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //2. Trả về dữ liệu
            return customer;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            //1. Tạo kết nối và truy vấn
            var customer = _dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //2. Trả về dữ liệu
            return customer;
        }

        public int InsertCustomer(Customer customer)
        {
            //1. Duyệt các thuộc tính trên customer và tạo parameters
            var parameters = MappingDbType<Customer>(customer);

            //2. Kết nối tới CSDL:
            var rowAffects = _dbConnection.Execute("Proc_InsertCustomer", parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int UpdateCustomer(Guid customerId, Customer customer)
        {
            customer.CustomerId = customerId;

            //1. Duyệt các thuộc tính trên customer và tạo parameters
            var parameters = MappingDbType<Customer>(customer);

            //2. Kết nối tới CSDL:
            int rowAffects = _dbConnection.Execute("Proc_UpdateCustomer", param: parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int DeleteCustomer(Guid customerId)
        {
            //1. Kết nối tới CSDL:
            int rowAffects = _dbConnection.Execute("Proc_DeleteCustomerById", param: new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);

            //2. Trả về số bản ghi bị ảnh hưởng
            return rowAffects;
        }

        private DynamicParameters MappingDbType<IEntity>(IEntity entity)
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
