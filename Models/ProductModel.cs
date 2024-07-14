using CUAHANG_TAPHOA.Areas.Admin.Views.Shared.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace CUAHANG_TAPHOA.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Tên Sản phẩm")]
        [MinLength(4, ErrorMessage = "Tên Sản phẩm phải có ít nhất 4 ký tự")]
        [Display(Name = "Tên Sản phẩm")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Mô tả Sản phẩm")]
        [MinLength(4, ErrorMessage = "Mô tả Sản phẩm phải có ít nhất 4 ký tự")]
        [Display(Name = "Mô tả Sản phẩm")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá Sản phẩm")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá Sản phẩm phải lớn hơn 0")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Giá Sản phẩm")]
        public decimal Price { get; set; }

        [Display(Name = "Thương hiệu")]
        public int BrandId { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }

        public string Image { get; set; }

        [NotMapped]
        [FileExtension]
        [Display(Name = "Hình ảnh")]
        public IFormFile? ImageUpload { get; set; }
    }
}
