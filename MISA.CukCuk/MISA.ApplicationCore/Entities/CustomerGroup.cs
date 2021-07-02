using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class CustomerGroup : BaseEntity
    {
        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid CustomerGroupId { get; set; }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        public string CustomerGroupName { get; set; }

        /// <summary>
        /// Id của lớp cha nhóm khách hàng
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
