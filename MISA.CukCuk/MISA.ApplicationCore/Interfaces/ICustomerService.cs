using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomers();

        Customer GetCustomerById(Guid customerId);

        Customer GetCustomerByCode(string customerCode);

        ServiceResult InsertCustomer(Customer customer);

        ServiceResult UpdateCustomer(Guid customerId, Customer customer);

        ServiceResult DeleteCustomer(Guid customerId);
    }
}
