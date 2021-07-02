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
using MISA.ApplicationCore.Interfaces.IRepositories;

namespace MISA.Infrastructure.Repository
{
    public class CustomerGroupRepository : BaseRepository<CustomerGroup>, ICustomerGroupRepository
    {
        #region Constructer
        public CustomerGroupRepository(IConfiguration configuration) : base(configuration)
        {
            
        }
        #endregion
    }
}
