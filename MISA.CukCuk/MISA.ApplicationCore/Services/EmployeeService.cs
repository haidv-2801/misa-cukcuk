using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository _employeeRepository;

        #region Constructer
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Method
        /// <summary>
        // Lấy toàn bộ nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public IEnumerable<Employee> GetEmployees()
        {
            var employees = _employeeRepository.GetEmployees();
            return employees;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <returns>Null -  không tìm thấy</returns>
        public Employee GetEmployeeByCode(string employeeCode)
        {
            var employee = _employeeRepository.GetEmployeeByCode(employeeCode);
            return employee;
        }

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="Employee">Thông tin nhân viên</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public ServiceResult InsertEmployee(Employee Employee)
        {
            var serviceResult = new ServiceResult();
            var rowAffects = _employeeRepository.InsertEmployee(Employee);

            //Validate dữ liệu

            if (rowAffects > 0)
            {
                serviceResult.Messasge = "Thêm nhân viên thành công";
                serviceResult.MISACode = MISACode.Success;
                serviceResult.Data = rowAffects;
            }

            return serviceResult;
        }

        public Employee GetEmployeeById(Guid EmployeeId)
        {
            var employee = _employeeRepository.GetEmployeeById(EmployeeId);
            return employee;
        }

        public ServiceResult UpdateEmployee(Guid employeeId, Employee employee)
        {
            var serviceResult = new ServiceResult();
            var rowAffects = _employeeRepository.UpdateEmployee(employeeId, employee);
            if (rowAffects > 0)
            {
                serviceResult.Messasge = "Sửa nhân viên thành công";
                serviceResult.MISACode = MISACode.Success;
                serviceResult.Data = employee;
            }
            return serviceResult;
        }

        public ServiceResult DeleteEmployee(Guid employeeId)
        {
            var serviceResult = new ServiceResult();

            serviceResult.Data = _employeeRepository.DeleteEmployee(employeeId);

            return serviceResult;
        }
        #endregion
    }
}
