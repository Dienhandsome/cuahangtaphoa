using Microsoft.AspNetCore.Identity;

namespace CUAHANG_TAPHOA.Models
{
	public class AppUserModel: IdentityUser
	{
		public string Occupation {  get; set; }
		public string RoleId {  get; set; }
	}
}
