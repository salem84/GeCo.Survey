using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace GeCoSurvey.Web.Areas.Admin.Models.Users
{
	public class CreateUserViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		[Required, Email]
		public string Email { get; set; }

		[Display(Name = "Domanda Segreta")]
		public string PasswordQuestion { get; set; }

		[StringLength(100)]
		[Display(Name = "Risposta Segreta")]
		public string PasswordAnswer { get; set; }
	}
}