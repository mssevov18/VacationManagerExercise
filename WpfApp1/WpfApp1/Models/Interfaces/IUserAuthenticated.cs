using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelLibrary.Models.Data;

namespace Application.Models.Interfaces
{
	public interface IUserAuthenticated
	{
		public User AuthenticatedUser { get; set; }
		public bool LoggedIn { get; set; }
	}
}
