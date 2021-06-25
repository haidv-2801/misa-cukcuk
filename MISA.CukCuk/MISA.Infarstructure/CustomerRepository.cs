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
            var customerContext = new CustomerContext();
            return 1;
        }

        public Customer GetCustomerByCode(string customerCode)
        {
            var customerContext = new CustomerContext();
            var customer = customerContext.GetCustomerByCode(customerCode);
            return customer;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            var customerContext = new CustomerContext();
            var customer = customerContext.GetCustomerById(customerId);
            return customer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var customerContext = new CustomerContext();
            var customer = customerContext.GetCustomers();
            return customer;
        }

        public int InsertCustomer(Customer customer)
        {
            var customerContext = new CustomerContext();
            var rowAffects = customerContext.InsertCustomer(customer);
            return rowAffects;
        }

        public int UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
