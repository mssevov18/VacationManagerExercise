using Application.Models.Interfaces;
using ModelLibrary.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
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
	public partial class ListUsers : UserControl, IControllable, IUserAuthenticated, IPaginated
	{
		List<User> _queriedUsers = new List<User>();
		List<User> _selectedUsers = new List<User>();
		PaginationFooter _paginator;
		bool _loading;
		string _password = string.Empty;

		public ListUsers()
		{
			_loading = true;
			InitializeComponent();

			FooterFrame.Content = _paginator = new PaginationFooter(this);
			_loading = false;
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
				{
					PopupFirstNameBox.IsEnabled = _authUser.CanEditUsers;
					PopupLastNameBox.IsEnabled = _authUser.CanEditUsers;
					PopupRoleBox.IsEnabled = _authUser.CanEditUsers;
					AddUser.Visibility = _authUser.CanEditUsers ? Visibility.Visible : Visibility.Hidden;
					AddUserPopup.Visibility = Visibility.Collapsed;
					DeleteUserPopup.Visibility = Visibility.Collapsed;
					EditUserPopup.Visibility = _authUser.CanEditUsers ? Visibility.Visible : Visibility.Hidden;
					LoggedIn = true;
				}
			}
		}
		public bool LoggedIn { get; set; }

		public User? PublicUser { get; set; }

		public int CollectionSize => _queriedUsers.Count;

		public void Clear()
		{
			PublicUser = null;
			_queriedUsers = null;
			_selectedUsers = null;
			FilterCombo.SelectedIndex = 0;
			SortCombo.SelectedIndex = 0;
			SeachTextBox.Clear();
			UsersList.ItemsSource = null;
			_password = string.Empty;
			_paginator.Reset();
			PopupPasswordBox.Clear();
			PopupPasswordConfirmBox.Clear();
			AddUserPopup.Visibility = Visibility.Collapsed;
			DeleteUserPopup.Visibility = Visibility.Collapsed;
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				_queriedUsers = dbContext.Users.Where(u => u.IsDeleted == false).ToList();
				_paginator.Reset();
				UpdateCollection();
			}
		}

		private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_loading)
				UpdateCollection();
		}

		private void SeachTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			UpdateCollection();
		}

		private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (_loading)
				return;

			if (SortCombo.SelectedItem != null)
			{
				Func<User, object> OrderFunc = (User u) => u.Id;
				string unboxedTag = ((ComboBoxItem)SortCombo.SelectedItem).Tag.ToString();
				if (unboxedTag != null)
				{
					switch (unboxedTag[0])
					{
						case 'R':
							OrderFunc = (User u) => u.Role;
							break;
						case 'F':
							OrderFunc = (User u) => u.FirstName;
							break;
						case 'L':
							OrderFunc = (User u) => u.LastName;
							break;
						case 'U':
							OrderFunc = (User u) => u.Username;
							break;
					}
					_queriedUsers = _queriedUsers.OrderBy(u => u.Id).ToList();
					if (unboxedTag.Length > 1)
					{
						if (unboxedTag[1] == 'D')
							_queriedUsers = _queriedUsers.OrderByDescending(OrderFunc).ToList();
						else
							_queriedUsers = _queriedUsers.OrderBy(OrderFunc).ToList();
					}
					else
						_queriedUsers = _queriedUsers.OrderBy(OrderFunc).ToList();
				}
			}
			UpdateCollection();
		}

		public void UpdateCollection()
		{
			int oldPageNum = _paginator.PageNumber;
			_selectedUsers = _queriedUsers
				.Where(u =>
					(SeachTextBox.Text == string.Empty ?
						true :
						u.Username.Contains(SeachTextBox.Text) &&
					(((ComboBoxItem)FilterCombo.SelectedItem).Tag.ToString() == "C" ?
						u.Role == "C" :
						true) &&
					(((ComboBoxItem)FilterCombo.SelectedItem).Tag.ToString() == "D" ?
						u.Role == "D" :
						true) &&
					(((ComboBoxItem)FilterCombo.SelectedItem).Tag.ToString() == "L" ?
						u.Role == "L" :
						true) &&
					(((ComboBoxItem)FilterCombo.SelectedItem).Tag.ToString() == "U" ?
						u.Role == "U" :
						true)))
				.Skip((_paginator.PageNumber - 1) * _paginator.PageSize)
				.Take(_paginator.PageSize)
				.ToList();
			UsersList.ItemsSource = _selectedUsers;
		}
		private void OpenMoreInfoButton_Click(object sender, RoutedEventArgs e)
		{
			PublicUser = _queriedUsers.FirstOrDefault(qu => qu.Id == ((User)((Button)sender).DataContext).Id);
			if (PublicUser == null)
				return;

			AccessUserPopup.DataContext = PublicUser;
			AddUserPopup.Visibility = Visibility.Collapsed;
			DeleteUserPopup.Visibility = Visibility.Visible;
			AccessUserPopup.IsOpen = true;
		}

		private void CloseAccessUserPopup_Click(object sender, RoutedEventArgs e)
		{
			AccessUserPopup.IsOpen = false;
			PublicUser = null;
			PopupUsernameBox.IsEnabled = false;
			DeleteUserPopup.Visibility = Visibility.Collapsed;
			AddUserPopup.Visibility = Visibility.Collapsed;
			EditUserPopup.Visibility = Visibility.Visible;
		}

		private void CreateUser()
		{
			if (PopupFirstNameBox.Text == string.Empty ||
	PopupLastNameBox.Text == string.Empty ||
	PopupUsernameBox.Text == string.Empty ||
	PopupRoleBox.Text == string.Empty ||
	!_authUser.CanEditUsers)
				return;

			User user = new User();
			user.FirstName = PopupFirstNameBox.Text;
			user.LastName = PopupLastNameBox.Text;
			user.Username = PopupUsernameBox.Text;
			switch (PopupRoleBox.Text)
			{
				case "CEO":
					user.Role = "C";
					break;
				case "Team Lead":
					user.Role = "L";
					break;
				case "Developer":
					user.Role = "D";
					break;
				default:
					user.Role = "U";
					break;
			}
			if (_password == string.Empty)
				return;
			user.Password = _password;

			bool success = false;
			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				//PopupFirstNameBox, PopupLastNameBox, PopupUsernameBox, PopupRoleBox
				if (dbContext.Users.Where(u => u.Username == PopupUsernameBox.Text).Count() == 0)
				{
					success = true;
					dbContext.Users.Add(user);
					dbContext.SaveChanges();
				}
			}
			if (success)
			{
				_password = string.Empty;
				PopupPasswordBox.Clear();
				PopupPasswordConfirmBox.Clear();

				EnterPasswordPopup.IsOpen = false;
				AccessUserPopup.IsOpen = false;

				_queriedUsers.Add(user);
				UpdateCollection();
			}
		}

		private void EditUserPopup_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Not stable");

			//if (PopupFirstNameBox.Text == string.Empty ||
			//	PopupLastNameBox.Text == string.Empty ||
			//	PopupUsernameBox.Text == string.Empty ||
			//	PopupRoleBox.Text == string.Empty)
			//	return;

			//using (VacationManagerContext dbContext = new VacationManagerContext())
			//{
			//	//PopupFirstNameBox, PopupLastNameBox, PopupUsernameBox, PopupRoleBox
			//	User temp = dbContext.Users.Where(u => u.Username == PopupUsernameBox.Text).FirstOrDefault();
			//	temp.FirstName = PopupFirstNameBox.Text;
			//	temp.LastName = PopupLastNameBox.Text;
			//	temp.Role = PopupRoleBox.Text;
			//	_queriedUsers.FirstOrDefault(u => u.Username == PopupUsernameBox.Text).FirstName = PopupFirstNameBox.Text;
			//	_queriedUsers.FirstOrDefault(u => u.Username == PopupUsernameBox.Text).LastName = PopupLastNameBox.Text;
			//	_queriedUsers.FirstOrDefault(u => u.Username == PopupUsernameBox.Text).Role = PopupRoleBox.Text;
			//	dbContext.SaveChanges();
			//}
		}
		private void AddUserPopup_Click(object sender, RoutedEventArgs e)
		{
			if (PopupFirstNameBox.Text == string.Empty ||
				PopupLastNameBox.Text == string.Empty ||
				PopupUsernameBox.Text == string.Empty ||
				PopupRoleBox.Text == string.Empty ||
	!_authUser.CanEditUsers)
				return;

			PopupPasswordBox.Clear();
			PopupPasswordConfirmBox.Clear();
			EnterPasswordPopup.IsOpen = true;
		}

		private void OpenEmptyPopupButton_Click(object sender, RoutedEventArgs e)
		{
			PopupUsernameBox.IsEnabled = true;
			PublicUser = new User();
			AccessUserPopup.DataContext = PublicUser;
			AccessUserPopup.IsOpen = true;
			DeleteUserPopup.Visibility = Visibility.Collapsed;
			EditUserPopup.Visibility = Visibility.Collapsed;
			AddUserPopup.Visibility = Visibility.Visible;
		}

		private void SubmitPasswordsButton_Click(object sender, RoutedEventArgs e)
		{
			if (PopupPasswordBox.Password == null || PopupPasswordConfirmBox.Password == null)
			{
				MessageBox.Show("Passwords don't match");
				return;
			}

			if (PopupPasswordBox.Password == PopupPasswordConfirmBox.Password)
			{
				_password = PopupPasswordBox.Password;
				CreateUser();
			}
		}

		private void DeleteUserPopup_Click(object sender, RoutedEventArgs e)
		{
			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				dbContext.Users.Where(u => u.Id == ((User)((Button)sender).DataContext).Id).FirstOrDefault().IsDeleted = true;
				dbContext.SaveChanges();
				_queriedUsers.Remove(_queriedUsers.Where(u => u.Id == ((User)((Button)sender).DataContext).Id).FirstOrDefault());
			}
			AccessUserPopup.IsOpen = false;
			UpdateCollection();
		}
	}
}
