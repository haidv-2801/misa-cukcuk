using MISA.ApplicationCore.Entities;
using MISA.Infarstructure.Models;
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

        ServiceResult UpdateCustomer(Customer customer);

        ServiceResult DeleteCustomer(Guid customerId);
    }
}
