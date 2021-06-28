using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Data;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Entities;

namespace MISA.CukCuk.Web.Controllers
{
    public class CustomersController : BaseEntityController<Customer>
    {
        #region Declare
        IBaseService<Customer> _baseService;
        #endregion

        #region Methods
        public CustomersController(IBaseService<Customer> baseService) : base(baseService)
        {
            _baseService = baseService;
        }
        #endregion
    }
}
