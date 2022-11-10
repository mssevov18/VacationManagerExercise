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
		public User AuthenticatedUser { get => _authUser; set => _authUser = value; }

		public int CollectionSize => _queriedUsers.Count;

		public void Clear()
		{
			_paginator.Reset();
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

		private void OpenMoreInfoButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_loading)
				UpdateCollection();
		}

		private void SeachTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (!_loading)
				UpdateCollection();
		}

		private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_loading)
				UpdateCollection();
		}

		public void UpdateCollection()
		{
			UsersList.ItemsSource = _queriedUsers
				.Where(u =>
				(SeachTextBox.Text == string.Empty ? true : u.Username.Contains(SeachTextBox.Text))
				/*add more for filter*/
				)
				.Skip((_paginator.PageNumber - 1) * _paginator.PageSize)
				.Take(_paginator.PageSize);
		}
	}
}
