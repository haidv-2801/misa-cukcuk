using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface danh mục nhân viên
    /// </summary>
    /// DVHAI (25/06/2021)
    public interface IEmployeeRepository:IBaseRepository<Employee>
    {
        Employee GetEmployeeByCode(string employeeCode);
    }
}
