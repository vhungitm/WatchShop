namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feedback")]
    public partial class Feedback
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên!")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email!")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [StringLength(50)]
        public string Address { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Vui lòng nhập nội dung!")]
        public string Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool Status { get; set; }
    }
}
