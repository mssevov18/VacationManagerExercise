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
	/// Interaction logic for NavigationHeader.xaml
	/// </summary>
	public partial class NavigationHeader : UserControl, IControllable, IUserAuthenticated
	{
		public IWindowContainer WindowContainer { get; set; }

		public NavigationHeader() => _ClassInit();
		public NavigationHeader(IWindowContainer container)
		{
			WindowContainer = container;
			_ClassInit();
		}

		private void _ClassInit()
		{
			InitializeComponent();
		}

        public Tuple<Type, object>? Data => null;

		private User _authUser;
		public User AuthenticatedUser
		{
			get => _authUser;
			set
			{
				_authUser = value;
				if (value != null)
					LoggedIn = true;
			}
		}
		public bool LoggedIn { get; set; }

		public void Clear()
        {
			return;
        }

		private void LogOut(object sender, RoutedEventArgs e)
		{
			if (WindowContainer != null)
				WindowContainer.LogOut();
		}
	}
}
