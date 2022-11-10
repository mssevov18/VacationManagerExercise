using Application.Models.Interfaces;
using ModelLibrary.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Application.Models.UserControls
{
	/// <summary>
	/// Interaction logic for ListUsers.xaml
	/// </summary>
	public partial class ListUsers : UserControl, IControllable, IUserAuthenticated
	{
		public ListUsers()
		{
			InitializeComponent();
		}

		public Tuple<Type, object>? Data => null;

		private User _authUser;
		public User AuthenticatedUser { get => _authUser; set => _authUser = value; }

		public void Clear()
		{
			
		}
	}
}
