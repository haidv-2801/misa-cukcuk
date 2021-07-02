using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.IRepositories;
using MISA.ApplicationCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Services
{
    public class CustomerGroupService : BaseService<CustomerGroup>, ICustomerGroupService
    {
        #region Declare
        ICustomerGroupRepository _customerGroupRepository;
        #endregion

        #region Constructer
        public CustomerGroupService(ICustomerGroupRepository customerGroupRepository) : base(customerGroupRepository)
        {
            _customerGroupRepository = customerGroupRepository;
        }
        #endregion
    }
}
