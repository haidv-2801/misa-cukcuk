using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetEmployees();

        Employee GetEmployeeById(Guid employeeId);

        Employee GetEmployeeByCode(string employeeCode);

        ServiceResult InsertEmployee(Employee employee);

        ServiceResult UpdateEmployee(Guid employeeId, Employee employee);

        ServiceResult DeleteEmployee(Guid employeeId);
    }
}
