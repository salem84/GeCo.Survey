using System.Collections.Generic;
using System.Web.Security;
using PagedList;

namespace GeCoSurvey.Web.Areas.Admin.Models.Users
{
	public class IndexViewModel
	{
		public IPagedList<MembershipUser> Users { get; set; }
		public IEnumerable<string> Roles { get; set; }
		public bool IsRolesEnabled { get; set; }
	}
}