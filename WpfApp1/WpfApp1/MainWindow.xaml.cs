using Application.Models.Interfaces;
using Application.Models.Pages;
using ModelLibrary.Models.Data;
using Newtonsoft.Json.Linq;
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

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IWindowContainer, IUserAuthenticated
	{
		private Dictionary<string, IInterpagable> _pages = new Dictionary<string, IInterpagable>();
		private string loadedPage = "";

		public IInterpagable this[string pageName]
		{
			get
			{
				if (!_pages.ContainsKey(pageName))
					throw new ArgumentOutOfRangeException("pageName", $"Page \"{pageName}\" not in collection");
				return _pages[pageName];
			}
		}

		public Dictionary<string, IInterpagable> Pages { get => _pages; }

		private User _authUser;
		public User AuthenticatedUser
		{
			get => _authUser;
			set
			{
				_authUser = value;
				foreach (KeyValuePair<string, IInterpagable> authPages in _pages)
				{
					if (authPages.Key != loadedPage)
					{
						((IUserAuthenticated)authPages.Value).AuthenticatedUser = value;
						((IUserAuthenticated)authPages.Value).LoggedIn = value != null;
					}
				}
			}
		}
		public bool LoggedIn { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			_pages.Add("login", new LogInPage(this));
			_pages.Add("landing", new LandingPage(this));

			using (VacationManagerContext dbContext = new VacationManagerContext())
			{
				if (dbContext.Users.Count() == 0)
				{
					dbContext.Users.Add(new User()
					{
						FirstName = "Martin",
						LastName = "Sevov",
						Username = "admin",
						Password = "admin",
						IsDeleted = false,
						Role = "CEO"
					});
					dbContext.SaveChanges();
				}
			}

			ChangePage("login");

		}

		public void ChangePage(string pageName)
		{
			if (_pages.ContainsKey(pageName))
			{
				if (_pages.ContainsKey(loadedPage))
					_pages[loadedPage].Close();
				PageFrame.Content = _pages[loadedPage = pageName];
				this.Width = _pages[loadedPage].Width;
				this.Height = _pages[loadedPage].Height;
			}
		}

		public void LogOut()
		{
			loadedPage = "";
			LoggedIn = false;
			//foreach (KeyValuePair<string, IInterpagable> authPages in _pages)
			//	if (authPages.Key != loadedPage)
			//		((IUserAuthenticated)authPages.Value).LoggedIn = LoggedIn;
			AuthenticatedUser = null;
			ChangePage("login");
		}
	}
}
