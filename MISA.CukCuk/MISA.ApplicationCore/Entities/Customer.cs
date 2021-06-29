using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    // <summary>
    /// Khách hàng
    /// </summary>
    /// CreatedBy: DVHAI (24/06/2021)
    public class Customer
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        [Key]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [IRequired]
        [CheckDuplicate]
        [Display(Name = "Mã khách hàng")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [Display(Name = "Tên khách hàng")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Display(Name = "Địa chỉ")]
        public string Adress { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [CheckDuplicate]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính (0-Nữ, 1-Nam, 2-Khác)
        /// </summary>
        [Display(Name = "Giới tính")]
        public int? Gender { get; set; }

        /// <summary>
        /// Email khách hàng
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Khóa ngoại đến nhóm khách hàng
        /// </summary>
        [Display(Name = "Nhóm khách hàng")]
        public Guid? CustomerGroupId { get; set; }

        /// <summary>
        /// Số thẻ thành viên
        /// </summary>
        [Display(Name = "Mã thẻ thành viên")]
        public string MemberCardCode { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        [Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Mã số thuế công ty
        /// </summary>
        [Display(Name = "Mã số thuế")]
        public string CompanyTaxCode { get; set; }
        #endregion
    }
}
