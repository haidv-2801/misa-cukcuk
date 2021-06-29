using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        IBaseRepository<Employee> _employeeRepository;

        #region Constructer
        public EmployeeService(IBaseRepository<Employee> employeeRepository):base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Methods
        public override int Insert(Employee entity)
        {
            //1. Validate dữ liệu
            var isValid = true;

            //2.
            if (isValid)
                return base.Insert(entity);
            //_employeeRepository.Insert(entity);

            return -1;
        }

        public IEnumerable<Employee> GetEmployeesPaging(int limit, int offset)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
