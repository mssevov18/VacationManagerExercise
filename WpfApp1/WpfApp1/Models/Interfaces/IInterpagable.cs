using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Application.Models.Interfaces
{
	public interface IInterpagable
	{
		public string Name { get; }

		public IWindowContainer WindowOwner { get; set; }

		public int Width { get; }
		public int Height { get; }

		public void Close();
		public void RequestPageChange(string pageName);
	}
}
