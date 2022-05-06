namespace Model.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("Banner")]
    public partial class Banner
    {
        public long ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề!")]
        [MaxLength(50, ErrorMessage = "Tiêu đề không được vượt quá 50 ký tự!")]
        public string Title { set; get; }

        [Required(ErrorMessage = "Vui lòng chọn hình ảnh!")]
        public string Image { set; get; }

        public string Link { set; get; }

        public int? DisplayOrder { set; get; }

        public bool Status { set; get; }
    };
}
