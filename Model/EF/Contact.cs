namespace Model.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("Contact")]
    public partial class Contact
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Vui lòng nhập nội dung!")]
        [AllowHtml]
        public string Content { get; set; }

        public bool Status { get; set; }
    }
}
