using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace MISA.Infarstructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        #region Constructer
        public CustomerRepository(IConfiguration configuration):base(configuration)
        {
        }
        #endregion

        #region Methods
        public Customer GetCustomerByCode(string customerCode)
        {
            var customer = GetEntityByProperty("CustomerCode", customerCode);
            return customer;
        }
        #endregion
    }
}
