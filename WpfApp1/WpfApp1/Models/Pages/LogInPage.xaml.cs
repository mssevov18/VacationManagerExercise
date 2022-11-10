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
	public partial class LogInPage : Page, IInterpagable
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

		int IInterpagable.Width => 400;
		int IInterpagable.Height => 600;

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
			if (UsernameBox.Text == String.Empty ||
				PasswordBox.Password == String.Empty)
				return;

			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				try
				{
					Tuple<string, string> tuple = dbContext
						.Users
						.Where(u => u.IsDeleted == false &&
								   u.Username == UsernameBox.Text &&
								   u.Password == PasswordBox.Password)
						.Select(u =>
							new Tuple<string, string>(u.Username, u.Password))
						.FirstOrDefault();

					if (tuple.Item1 !=  UsernameBox.Text &&
						tuple.Item2 != PasswordBox.Password)
						throw new Exception($"Invalid token");

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
