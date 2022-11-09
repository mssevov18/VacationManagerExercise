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

namespace Application.Models.Pages
{
	/// <summary>
	/// Interaction logic for LogInPage.xaml
	/// </summary>
	public partial class LogInPage : Page, IInterpagable
	{
		public LogInPage()
		{
			InitializeComponent();
		}
		public LogInPage(IWindowContainer windowContainer)
		{
			_windowOwner = windowContainer;

			InitializeComponent();
		}

		IWindowContainer _windowOwner;
		public IWindowContainer WindowOwner { get => _windowOwner; set => _windowOwner = value; }

		int IInterpagable.Width => 600;
		int IInterpagable.Height => 800;

		public void Close()
		{
			UsernameBox.Clear();
			PasswordBox.Clear();
		}

		public void RequestPageChange(string pageName)
		{
			throw new NotImplementedException();
		}

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			if (UsernameBox.Text == String.Empty ||
				PasswordBox.Password == String.Empty)
				return;

			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				MessageBox.Show(dbContext
					.Users
					.Where(u=>u.IsDeleted==false)
					.Where(u=>u.Username == UsernameBox.Text &&
							  u.Password == PasswordBox.Password)
					.Count().ToString());
			}
		}
	}
}
