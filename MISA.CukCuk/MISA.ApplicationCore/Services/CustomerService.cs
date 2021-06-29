using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Declare
        ICustomerRepository _customerRepository;
        #endregion

        #region Constructer
        public CustomerService(ICustomerRepository customerRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region Method
        public override ServiceResult Insert(Customer customer)
        {
            return base.Insert(customer);
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
