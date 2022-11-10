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

namespace Application.Models.UserControls
{
	/// <summary>
	/// Interaction logic for PaginationFooter.xaml
	/// </summary>
	public partial class PaginationFooter : UserControl
	{
		public IPaginated Owner { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		private bool _loading;
		public PaginationFooter() => _ClassInit();
		public PaginationFooter(IPaginated Owner)
		{
			this.Owner = Owner;
			_ClassInit();
		}

		private void _ClassInit()
		{
			_loading = true;

			InitializeComponent();

			_loading = false;
		}

		public void Reset()
		{
			PageNumber = 1;
			PageSize = 5;
			SizeBox.SelectedIndex = 0;
			UpdateUC();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Owner == null || _loading == true || SizeBox.SelectedItem == null)
				return;
			int tempSize = PageSize;
			PageSize = Convert.ToInt32(((ComboBoxItem)SizeBox.SelectedItem).Tag.ToString());
			if (Owner.CollectionSize / PageSize < 1)
			{
				FirstButton.IsEnabled = false;
				PrevButton.IsEnabled = false;
				NextButton.IsEnabled = false;
				LastButton.IsEnabled = false;
			}
			else
			{
				FirstButton.IsEnabled = true;
				PrevButton.IsEnabled = true;
				NextButton.IsEnabled = true;
				LastButton.IsEnabled = true;
			}
			PageNumber = (tempSize * PageNumber) / PageSize + 1;
			//if (PageNumber > 1 + Owner.CollectionSize / PageSize)
			//	PageNumber = 1 + Owner.CollectionSize / PageSize;
			UpdateUC();
		}

		private void GoFirst(object sender, RoutedEventArgs e)
		{
			PageNumber = 1;
			UpdateUC();
		}

		private void GoPrevious(object sender, RoutedEventArgs e)
		{
			if (PageNumber != 1)
				PageNumber--;
			UpdateUC();
		}

		private void GoNext(object sender, RoutedEventArgs e)
		{
			if (PageNumber != 1 + Owner.CollectionSize / PageSize)
				PageNumber++;
			UpdateUC();
		}
		private void GoLast(object sender, RoutedEventArgs e)
		{
			PageNumber = 1 + Owner.CollectionSize / PageSize;
			UpdateUC();
		}

		private void UpdateUC()
		{
			PrevButton.Content = $"<[{(PageNumber == 1 ? PageNumber : PageNumber - 1)}]<";
			CurrentButton.Content = $"[{PageNumber}]";
			NextButton.Content = $">[{(PageNumber == Owner.CollectionSize / PageSize + 1 ? PageNumber : PageNumber + 1)}]>";
			LastButton.Content = $">|[{Owner.CollectionSize / PageSize + 1}]";
			Owner.UpdateCollection();
		}
	}
}
