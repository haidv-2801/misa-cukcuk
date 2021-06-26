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

namespace MISA.Infarstructure
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Declare
        IConfiguration _configuration;
        #endregion

        #region
        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Methods
        public IEnumerable<Customer> GetCustomers()
        {
            //1. Kết nối tới CSDL:
            var connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");

            //2. Tạo kết nối và truy vấn
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure).ToList();

            //3. Trả về dữ liệu
            return customers;
        }

        public Customer GetCustomerByCode(string customerCode)
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            //2. Tạo kết nối và truy vấn
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //3. Trả về dữ liệu
            return customer;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";

            //2. Tạo kết nối và truy vấn
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //3. Trả về dữ liệu
            return customer;
        }

        public int InsertCustomer(Customer customer)
        {
            //1. Duyệt các thuộc tính trên customer và tạo parameters
            var properties = customer.GetType().GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(customer);
                var propertyType = property.PropertyType;

                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                else
                    parameters.Add($"@{propertyName}", propertyValue);
            }

            //2. Tạo kết nối và truy vấn
            var connectionString = "User Id=dev;" +
                                    "Host=47.241.69.179;" +
                                    "Port=3306;" +
                                    "Password=12345678;" +
                                    "Database=MISACukCuk_Demo;" +
                                    "Character Set=utf8";

            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var rowAffects = dbConnection.Execute("Proc_InsertCustomer", parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int UpdateCustomer(Guid customerId, Customer customer)
        {
            customer.CustomerId = customerId;
            //1. Duyệt các thuộc tính trên customer và tạo parameters
            var properties = customer.GetType().GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(customer);
                var propertyType = property.PropertyType;

                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                else
                    parameters.Add($"@{propertyName}", propertyValue);
            }

            //2. Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";

            IDbConnection dbConnection = new MySqlConnection(connectionString);
            int rowAffects = dbConnection.Execute("Proc_UpdateCustomer", param: parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int DeleteCustomer(Guid customerId)
        {
            //1. Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";

            IDbConnection dbConnection = new MySqlConnection(connectionString);
            int rowAffects = dbConnection.Execute("Proc_DeleteCustomerById", param: new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);

            //3. Trả về số bản ghi bị ảnh hưởng
            return rowAffects;
        }
        #endregion
    }
}
