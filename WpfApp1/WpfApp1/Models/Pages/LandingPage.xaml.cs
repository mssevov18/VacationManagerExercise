using Application.Models.Interfaces;
using Application.Models.UserControls;
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
	/// Interaction logic for LandingPage.xaml
	/// </summary>
	public partial class LandingPage : Page, IInterpagable, IUserAuthenticated, IControlling
	{
		public LandingPage() => _ClassInit();
		public LandingPage(IWindowContainer WindowContainer)
		{
			this.WindowOwner = WindowContainer;
			_ClassInit();
		}

		private void _ClassInit()
		{
			InitializeComponent();

			_ucontrols.Add("navigation", new NavigationHeader(this.WindowOwner));
			_ucontrols.Add("listusers", new ListUsers());


			HeaderFrame.Content = _ucontrols["navigation"];
			BodyFrame.Content = _ucontrols["listusers"];
		}

		private IWindowContainer _windowOwner;
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
				foreach (IUserAuthenticated authControls in _ucontrols.Values)
					authControls.AuthenticatedUser = value;
			}
		}
		public bool LoggedIn { get; set; }

		private Dictionary<string, IControllable> _ucontrols = new Dictionary<string, IControllable>();
		public Dictionary<string, IControllable> Controls => _ucontrols;
		public Tuple<Type, object>? this[string controlName]
		{
			get
			{
				if (!_ucontrols.ContainsKey(controlName))
					throw new ArgumentOutOfRangeException("controlName", $"Control \"{controlName}\" not in collection");
				return _ucontrols[controlName].Data;
			}
		}

		public void Close()
		{
			this.Clear();
		}

		public void RequestPageChange(string pageName)
		{
			_windowOwner.ChangePage(pageName);
		}

		public void Clear()
		{
			foreach (IControllable controllable in _ucontrols.Values)
				controllable.Clear();
		}
	}
}
