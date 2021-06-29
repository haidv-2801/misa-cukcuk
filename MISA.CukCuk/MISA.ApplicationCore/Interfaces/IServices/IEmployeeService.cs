using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {

        /// <summary>
        /// Danh sách nhân viên phân trang
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// CREATED BY: DVHAI (28/06/2021)
        public IEnumerable<Employee> GetEmployeesPaging(int limit, int offset);
    }
}
