namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
   
    [Table("Product")]
    public partial class Product
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm!")]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình ảnh!")]
        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá khuyến mãi!")]
        public decimal PromotionPrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục!")]
        public long CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }

        public bool Status { get; set; }

        public DateTime? TopHot { get; set; }
    }
}
