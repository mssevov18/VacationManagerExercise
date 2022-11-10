using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Interfaces
{
    public interface IControlling
    {
        public Dictionary<string, IControllable> Controls { get; }
        public Tuple<Type, object>? this[string controlName] { get; }
        public void Clear();
    }
}
