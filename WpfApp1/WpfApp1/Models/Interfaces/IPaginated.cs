using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Interfaces
{
	public interface IPaginated
	{
		public void UpdateCollection();
		public int CollectionSize { get; }
	}
}
