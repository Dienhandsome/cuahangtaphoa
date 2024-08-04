using System.ComponentModel.DataAnnotations;

namespace CUAHANG_TAPHOA.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập UserName")]
		public string UserName { get; set; }
		
		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập pass word")] // mã hóa password
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
