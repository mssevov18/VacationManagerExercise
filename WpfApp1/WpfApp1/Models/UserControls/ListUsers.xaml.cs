using Application.Models.Interfaces;
using ModelLibrary.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		List<User> _queriedUsers = new List<User>();

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

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				_queriedUsers = dbContext.Users.ToList();
				UsersList.ItemsSource = _queriedUsers;
			}
		}

		private void OpenMoreInfoButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
