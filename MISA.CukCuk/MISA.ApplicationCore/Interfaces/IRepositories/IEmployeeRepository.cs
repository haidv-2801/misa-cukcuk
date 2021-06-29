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
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// DVHAI (25/06/2021)
        IEnumerable<Employee> GetEmployees();

        /// <summary>
        ///  Lấy nhân viên theo id
        /// </summary>
        /// <param name="employeeId">Id của nhân viên</param>
        /// <returns>Bản ghi thông tin 1 nhân viên</returns>
        /// DVHAI (25/06/2021)
        Employee GetEmployeeById(Guid employeeId);

        /// <summary>
        ///  Lấy nhân viên theo mã
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <returns>Bản ghi thông tin 1 nhân viên</returns>
        /// DVHAI (25/06/2021)
        Employee GetEmployeeByCode(string employeeCode);

        /// <summary>
        /// Thêm nhân viên
        /// </summary>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <returns>Số bản ghi</returns>
        int InsertEmployee(Employee employee);

        /// <summary>
        /// Cập nhập thông tin bản ghi
        /// </summary>
        /// <param name="employeeId">Id nhân viên</param>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// DVHAI (25/06/2021)
        int UpdateEmployee(Guid employeeId, Employee employee);

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// DVHAI (25/06/2021)
        int DeleteEmployee(Guid employeeId);
    }
}
