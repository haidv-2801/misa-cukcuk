using MISA.Infarstructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infarstructure
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        
        Customer GetCustomerById(Guid customerId);

        int InsertCustomer(Customer customer);

        int UpdateCustomer(Customer customer);

        int DeleteCustomer(Guid customerId);
    }
}
