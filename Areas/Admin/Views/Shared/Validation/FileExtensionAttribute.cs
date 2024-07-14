using System.ComponentModel.DataAnnotations;

namespace CUAHANG_TAPHOA.Areas.Admin.Views.Shared.Validation
{
    public class FileExtensionAttribute: ValidationAttribute
    //      FileExtension lấy tên từ đây
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file) 
            {
             var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpeg" };

                bool result = extension.Any(x => extension.EndsWith(x));

                if(!result) 
                {
                    return new ValidationResult("Allowed extension are jpg or png and jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
