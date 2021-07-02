using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MISA.ApplicationCore
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Declare
        ICustomerRepository _customerRepository;
        ICustomerGroupRepository _customerGroupRepository;
        List<CustomerGroup> _customerGroups = null;
        #endregion

        #region Constructer
        public CustomerService(ICustomerRepository customerRepository, ICustomerGroupRepository customerGroupRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _customerGroupRepository = customerGroupRepository;
            _customerGroups = _customerGroupRepository.GetEntities().ToList();
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra xem "nhóm khách hàng" trong tệp nhập khẩu có tồn tại trong database không
        /// </summary>
        /// <param name="customer">Bản ghi khách hàng</param>
        /// <returns>true-false</returns>
        protected override bool validateCustomerGroupCustom(Customer customer)
        {
            var customerGroup = _customerGroups.Exists(x => x.CustomerGroupName.Equals(customer.CustomerGroupName));
            
            if (customerGroup == false)
                customer.Status.Add($"Nhóm khách hàng không có trong hệ thống");

            return customerGroup;
        }

        public IEnumerable<Customer> GetCustomerPaging(int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomerByGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
