using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Web.Controllers
{
    public class CustomerGroupsController : BaseEntityController<CustomerGroup>
    {
        #region Declare
        ICustomerGroupService _customerGroupService;
        #endregion

        #region Methods
        public CustomerGroupsController(ICustomerGroupService customerGroupService) : base(customerGroupService)
        {
            _customerGroupService = customerGroupService;
        }
        #endregion
    }
}
