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

namespace MISA.Infrastructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
        #endregion

        #region
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);

        }
        #endregion

        #region Methods
        public IEnumerable<Employee> GetEmployees()
        {
            //1. Tạo kết nối và truy vấn
            var Employees = _dbConnection.Query<Employee>("Proc_GetEmployees", commandType: CommandType.StoredProcedure).ToList();

            //2. Trả về dữ liệu
            return Employees;
        }

        public Employee GetEmployeeByCode(string EmployeeCode)
        {
            //1. Tạo kết nối và truy vấn
            var Employee = _dbConnection.Query<Employee>("Proc_GetEmployeeByCode", new { EmployeeCode = EmployeeCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //2. Trả về dữ liệu
            return Employee;
        }

        public Employee GetEmployeeById(Guid EmployeeId)
        {
            //1. Tạo kết nối và truy vấn
            var Employee = _dbConnection.Query<Employee>("Proc_GetEmployeeById", new { EmployeeId = EmployeeId }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //2. Trả về dữ liệu
            return Employee;
        }

        public int InsertEmployee(Employee Employee)
        {
            //1. Duyệt các thuộc tính trên Employee và tạo parameters
            var parameters = MappingDbType<Employee>(Employee);

            //2. Kết nối tới CSDL:
            var rowAffects = _dbConnection.Execute("Proc_InsertEmployee", parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int UpdateEmployee(Guid EmployeeId, Employee Employee)
        {
            Employee.EmployeeId = EmployeeId;

            //1. Duyệt các thuộc tính trên Employee và tạo parameters
            var parameters = MappingDbType<Employee>(Employee);

            //2. Kết nối tới CSDL:
            int rowAffects = _dbConnection.Execute("Proc_UpdateEmployee", param: parameters, commandType: CommandType.StoredProcedure);

            //3. Trả về dữ liệu
            return rowAffects;
        }

        public int DeleteEmployee(Guid EmployeeId)
        {
            //1. Kết nối tới CSDL:
            int rowAffects = _dbConnection.Execute("Proc_DeleteEmployeeById", param: new { EmployeeId = EmployeeId }, commandType: CommandType.StoredProcedure);

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
