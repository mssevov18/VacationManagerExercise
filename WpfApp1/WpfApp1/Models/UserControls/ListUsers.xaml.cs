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
	public partial class ListUsers : UserControl, IControllable, IUserAuthenticated, IPaginated
	{
		List<User> _queriedUsers = new List<User>();
		List<User> _selectedUsers = new List<User>();
		PaginationFooter _paginator;
		bool _loading;

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
					PopupUsernameBox.IsEnabled = _authUser.CanEditUsers;
					PopupRoleBox.IsEnabled = _authUser.CanEditUsers;
					LoggedIn = true;
				}
			}
		}
		public bool LoggedIn { get; set; }

		public User? PublicUser { get; set; }

		public int CollectionSize => _queriedUsers.Count;

		public void Clear()
		{
			_paginator.Reset();
			PublicUser = null;
			_queriedUsers = null;
			_selectedUsers = null;
			FilterCombo.SelectedIndex = 0;
			SortCombo.SelectedIndex = 0;
			SeachTextBox.Clear();
			UsersList.ItemsSource = null;
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				_queriedUsers = dbContext.Users.ToList();
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
					if (unboxedTag.Length > 1)
					{
						if (unboxedTag[1] == 'D')
							_selectedUsers = _queriedUsers.OrderByDescending(OrderFunc).ToList();
						else
							_selectedUsers = _queriedUsers.OrderBy(OrderFunc).ToList();
					}
					else
						_selectedUsers = _queriedUsers.OrderBy(OrderFunc).ToList();
				}
			}
			UpdateCollection();
		}

		public void UpdateCollection()
		{
			_selectedUsers = _queriedUsers
				.Where(u =>
					(SeachTextBox.Text == string.Empty ?
						true :
						u.Username.Contains(SeachTextBox.Text)) &&
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
						true))
				.Skip((_paginator.PageNumber - 1) * _paginator.PageSize)
				.Take(_paginator.PageSize)
				.ToList();
			UsersList.ItemsSource = _selectedUsers;
		}
		private void OpenMoreInfoButton_Click(object sender, RoutedEventArgs e)
		{
			PublicUser = _queriedUsers.FirstOrDefault(qu => qu.Id == (((User)((Button)sender).DataContext)).Id);
			if (PublicUser == null)
				return;

			ViewUserPopup.DataContext = PublicUser;
			ViewUserPopup.IsOpen = true;
		}

		private void CloseViewUserPopup_Click(object sender, RoutedEventArgs e)
		{
			ViewUserPopup.IsOpen = false;
			PublicUser = null;
		}
	}
}
