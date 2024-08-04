using System.ComponentModel.DataAnnotations;
using System.Security;

namespace CUAHANG_TAPHOA.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage="Hãy nhập UserName")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Hãy nhập Email"),EmailAddress]
		public string Email {  get; set; }
		[DataType(DataType.Password),Required(ErrorMessage="Hãy nhập pass word")] // mã hóa password
		public string Password { get; set; }
	}
}
