using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Danh mục nhân viên
    /// </summary>
    /// CreatedBy: DVHAI (24/06/2021)
    public class Employee : BaseEntity
    {
        #region Property
        /// <summary>
        /// Id nhân viên
        /// </summary>
        [Key]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [IDuplicate]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Họ
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [IDuplicate]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [IDuplicate]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        public string Salary { get; set; }

        /// <summary>
        /// Tình trạng (0-nghỉ việc 1-Đang làm việc 2-Thực tập)
        /// </summary>
        public int WorkStatus { get; set; }

        /// <summary>
        /// Số chứng minh thư/căn cước công dân
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// ID phòng ban
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// Tên giới tính
        /// </summary>
        public string GenderName { get; set; }

        /// <summary>
        /// Tên tình trạng công việc
        /// </summary>
        public string WorkStatusName { get; set; }

        /// <summary>
        /// Id phòng ban
        /// </summary>
        public Guid? PositionId { get; set; }
        #endregion
    }
}
