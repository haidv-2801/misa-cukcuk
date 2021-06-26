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
    [Route("/v1/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: DVHAI 24/06/2021
        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeeService.GetEmployees();

            return Ok(employees);
        }

        /// <summary>
        /// Lấy khách hàng theo id
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <returns>Một khách hàng tìm được theo id</returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpGet("{employeeCode}")]
        public IActionResult Get(string employeeCode)
        {
            var employee = _employeeService.GetEmployeeByCode(employeeCode);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        /// <summary>
        /// Thêm một nhân viên mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {
            var serviceResult = _employeeService.InsertEmployee(employee);

            return Created("add", serviceResult);
        }

        /// <summary>
        /// Sửa một nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreateedBy: DVHAI 24/06/2021
        [HttpPut("{employeeId}")]
        public IActionResult Put([FromRoute] Guid employeeId, [FromBody] Employee employee)
        {
            var serviceResult = _employeeService.UpdateEmployee(employeeId, employee);
            return Ok(serviceResult);
        }

        // DELETE api/<employeesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var res = _employeeService.DeleteEmployee(id);
            return Ok(res);
        }
    }
}
