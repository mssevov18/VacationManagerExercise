using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Application.Models.Interfaces
{
	public interface IWindowContainer
	{
		public Dictionary<string, IInterpagable> Pages { get; }
		public IInterpagable this[string pageName] { get; }
		public void ChangePage(string pageName);
		public void LogOut();
	}
}
