using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MISA.Infarstructure;
using MISA.Infarstructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore
{
    public class CustomerService : ICustomerService
    {
        ICustomerRepository _customerRepository;

        #region Constructer
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region Method
        /// <summary>
        // Lấy toàn bộ khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public IEnumerable<Customer> GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            return customers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerCode">Mã nhân viên</param>
        /// <returns>Null -  không tìm thấy</returns>
        public Customer GetCustomerByCode(string customerCode)
        {
            var customer = _customerRepository.GetCustomerByCode(customerCode);
            return customer;
        }

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: DVHAI (24/06/2021)
        public ServiceResult InsertCustomer(Customer customer)
        {
            var serviceResult = new ServiceResult();
            var rowAffects = _customerRepository.InsertCustomer(customer);
            
            //Validate dữ liệu

            if(rowAffects > 0)
            {
                serviceResult.Messasge = "Thêm khách hàng thành công";
                serviceResult.MISACode = 100;
                serviceResult.Data = rowAffects;
            }

            return serviceResult;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public ServiceResult UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public ServiceResult DeleteCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
