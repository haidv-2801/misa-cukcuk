using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using MISA.Infarstructure.Models;
using System.Data;
using MISA.ApplicationCore;

namespace MISA.CukCuk.Web.Controllers
{
    [Route("/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: DVHAI 24/06/2021
        [HttpGet]
        public IActionResult Get()
        {
            var customerService = new CustomerService();
            var customers = customerService.GetCustomers();

            return Ok(customers);
        }

        /// <summary>
        /// Lấy khách hàng theo id
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <returns>Một khách hàng tìm được theo id</returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpGet("{customerCode}")]
        public IActionResult Get(string customerCode)
        {
            var customerService = new CustomerService();
            var customer = customerService.GetCustomerByCode(customerCode);

            if (customer == null)
                return NotFound();
            
            return Ok(customer);
        }

        /// <summary>
        /// Thêm một nhân viên mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            var customerService = new CustomerService();
            var rowAffects = customerService.InsertCustomer(customer);

            if (rowAffects > 0)
                return Ok(customer);
            else
                return NoContent();
        }

        /// <summary>
        /// Sửa một nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
