using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace GeCoSurvey.Web.Areas.Admin.Models.User
{
	public class CreateUserViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		[Required, Email]
		public string Email { get; set; }

		[Display(Name = "Secret Question")]
		public string PasswordQuestion { get; set; }

		[StringLength(100)]
		[Display(Name = "Secret Answer")]
		public string PasswordAnswer { get; set; }
	}
}