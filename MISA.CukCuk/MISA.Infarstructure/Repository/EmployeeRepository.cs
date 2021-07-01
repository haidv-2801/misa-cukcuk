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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructer
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public Employee GetEmployeeByCode(string employeeCode)
        {
            var employee = GetEntityByProperty("EmployeeCode", employeeCode);
            return employee;
        }
        #endregion
    }
}
