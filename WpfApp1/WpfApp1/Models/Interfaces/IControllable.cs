using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Application.Models.Interfaces
{
	public interface IControllable
	{
		public string Name { get; set; }

		public Tuple<Type, object>? Data { get; }
		public void Clear();
	}
}
