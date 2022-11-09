using Application.Models.Interfaces;
using Application.Models.Pages;
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

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IWindowContainer
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

		public MainWindow()
		{
			InitializeComponent();

			_pages.Add("login", new LogInPage());
			_pages.Add("landing", new LandingPage());

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
	}
}
