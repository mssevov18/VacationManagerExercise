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

		public IWindowContainer WindowOwner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		int IInterpagable.Width => throw new NotImplementedException();

		int IInterpagable.Height => throw new NotImplementedException();

		public void Close()
		{
			throw new NotImplementedException();
		}

		public void RequestPageChange(string pageName)
		{
			throw new NotImplementedException();
		}
	}
}
