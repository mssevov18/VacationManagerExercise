using Application.Models.Interfaces;
using ModelLibrary.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Application.Models.Pages
{
	/// <summary>
	/// Interaction logic for LogInPage.xaml
	/// </summary>
	public partial class LogInPage : Page, IInterpagable, IUserAuthenticated
	{
		public LogInPage()
		{
			InitializeComponent();
		}
		public LogInPage(IWindowContainer WindowContainer)
		{
			this.WindowOwner = WindowContainer;

			InitializeComponent();
		}

		IWindowContainer _windowOwner;
		public IWindowContainer WindowOwner { get => _windowOwner; set => _windowOwner = value; }

		int IInterpagable.Width => Convert.ToInt32(this.Width);
		int IInterpagable.Height => Convert.ToInt32(this.Height);

		private User _authUser;
		public User AuthenticatedUser
		{
			get => _authUser; 
			set 
			{
				_authUser = value;
				((IUserAuthenticated)_windowOwner).AuthenticatedUser = value;
			}
		}

		public void Close()
		{
			UsernameBox.Clear();
			PasswordBox.Clear();
		}

		public void RequestPageChange(string pageName)
		{
			_windowOwner.ChangePage(pageName);
		}

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			//Highlight incomplete inputs
			if (UsernameBox.Text == String.Empty ||
				PasswordBox.Password == String.Empty)
				return;

			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				try
				{
					User? tempUser = dbContext
						.Users
						.Where(u => u.IsDeleted == false &&
								   u.Username == UsernameBox.Text &&
								   u.Password == PasswordBox.Password)
						.FirstOrDefault();


					if (tempUser == null ||
						(tempUser.Username != UsernameBox.Text &&
						tempUser.Password != PasswordBox.Password))
						throw new Exception($"Invalid token");

					this.AuthenticatedUser = tempUser;
					this.RequestPageChange("landing");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
	}
}
