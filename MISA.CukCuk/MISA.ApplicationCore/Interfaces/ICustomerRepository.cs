using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Interface danh mục khách hàng
    /// </summary>
    /// DVHAI (25/06/2021)
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// DVHAI (25/06/2021)
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        ///  Lấy khách hàng theo id
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Bản ghi thông tin 1 khách hàng</returns>
        /// DVHAI (25/06/2021)
        Customer GetCustomerById(Guid customerId);

        /// <summary>
        ///  Lấy khách hàng theo mã
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Bản ghi thông tin 1 khách hàng</returns>
        /// DVHAI (25/06/2021)
        Customer GetCustomerByCode(string customerCode);

        /// <summary>
        /// Thêm khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns></returns>
        int InsertCustomer(Customer customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns></returns>
        /// DVHAI (25/06/2021)
        int UpdateCustomer(Guid customerId, Customer customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        /// DVHAI (25/06/2021)
        int DeleteCustomer(Guid customerId);
    }
}
