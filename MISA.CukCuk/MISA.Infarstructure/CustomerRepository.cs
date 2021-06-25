using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infarstructure
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Methods
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
            var customers = customerContext.GetCustomers();
            return customers;
        }

        public int InsertCustomer(Customer customer)
        {
            var customerContext = new CustomerContext();
            var rowAffects = customerContext.InsertCustomer(customer);
            return rowAffects;
        }

        public int UpdateCustomer(Guid customerId, Customer customer)
        {
            var customerContext = new CustomerContext();
            var rowAffects = customerContext.UpdateCustomer(customerId, customer);
            return rowAffects;
        }
        #endregion
    }
}
