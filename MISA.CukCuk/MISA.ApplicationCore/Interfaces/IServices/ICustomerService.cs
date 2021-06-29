using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface ICustomerService: IBaseService<Customer>
    {
        /// <summary>
        /// Lấy dữ liệu phân trang
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Danh sách khách hàng phân trang</returns>
        /// CREATED BY: DVHAI (28/06/2021)
        IEnumerable<Customer> GetCustomerPaging(int limit, int offset);

        /// <summary>
        /// Lấy danh sách khách hàng theo nhóm
        /// </summary>
        /// <param name="groupId">Id nhóm khách hàng</param>
        /// <returns>Danh sách khách hàng theo nhóm</returns>
        /// CREATED BY: DVHAI (28/06/2021)
        IEnumerable<Customer> GetCustomerByGroup(Guid groupId);
    }
}
