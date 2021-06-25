using MISA.Infarstructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using MySqlConnector;
using System.Data;
using System.Linq;

namespace MISA.Infarstructure
{
    public class CustomerContext
    {
        #region Methods
        /// <summary>
        // Lấy toàn bộ khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public IEnumerable<Customer> GetCustomers()
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure).ToList();
            return customers;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo mã
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Bản ghi thông tin khách hàng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public Customer GetCustomerByCode(string customerCode)
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo id
        /// </summary>
        /// <param name="customerId">Mã khách hàng</param>
        /// <returns>Bản ghi thông tin khách hàng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public Customer GetCustomerById(Guid customerId)
        {
            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return customer;
        }

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public int InsertCustomer(Customer customer)
        {
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

            var connectionString = "User Id=dev;" +
                                    "Host=47.241.69.179;" +
                                    "Port=3306;" +
                                    "Password=12345678;" +
                                    "Database=MISACukCuk_Demo;" +
                                    "Character Set=utf8";

            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var rowAffects = dbConnection.Execute("Proc_InsertCustomer", parameters, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        /// <summary>
        /// Sửa khách hàng
        /// </summary>
        /// <param name="customerId">Id nhân viên</param>
        /// <param name="customer">Thông tin khách hàng đã sửa</param>
        /// <returns></returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public int UpdateCustomer(Guid customerId, Customer customer)
        {
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

            // Kết nối tới CSDL:
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            dbConnection.Execute("Proc_UpdateCustomer", param: parameters, commandType: CommandType.StoredProcedure);

            return 1;
        }
        #endregion
    }
}
