using System.Collections.Generic;
using System.Web.Security;

namespace GeCoSurvey.Web.Areas.Admin.Models.User
{
	public class RoleViewModel
	{
		public string Role { get; set; }
		public IEnumerable<MembershipUser> Users { get; set; }
	}
}