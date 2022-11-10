using Application.Models.Interfaces;
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
	public partial class LandingPage : Page, IInterpagable
	{
		public LandingPage()
		{
			InitializeComponent();
		}
		public LandingPage(IWindowContainer WindowContainer)
		{
			this.WindowOwner = WindowContainer;
			InitializeComponent();
		}

		private IWindowContainer _windowOwner;
		public IWindowContainer WindowOwner { get => _windowOwner; set => _windowOwner = value; }

		int IInterpagable.Width => 800;

		int IInterpagable.Height => 450;

		public void Close()
		{

		}

		public void RequestPageChange(string pageName)
		{
			_windowOwner.ChangePage(pageName);
		}
	}
}
