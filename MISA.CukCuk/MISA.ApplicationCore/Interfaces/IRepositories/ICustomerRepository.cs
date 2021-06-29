using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Interface danh mục khách hàng
    /// </summary>
    /// DVHAI (25/06/2021)
    public interface ICustomerRepository:IBaseRepository<Customer>
    {
        /// <summary>
        ///  Lấy khách hàng theo mã
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Bản ghi thông tin 1 khách hàng</returns>
        /// DVHAI (25/06/2021)
        Customer GetCustomerByCode(string customerCode);
    }
}
