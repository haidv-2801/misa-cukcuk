using MISA.Infarstructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infarstructure
{
    public class CustomerRepository : ICustomerRepository
    {
        public int DeleteCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerById(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public int InsertCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public int UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
