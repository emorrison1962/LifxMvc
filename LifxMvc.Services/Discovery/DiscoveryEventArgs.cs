using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxMvc.Services.Discovery
{

	public class DiscoveryEventArgs : EventArgs
	{
		public DiscoveryContext DiscoveryContext { get; private set; }
		public DiscoveryEventArgs(DiscoveryContext discoveryContext)
		{
			this.DiscoveryContext = discoveryContext;
		}
	}

}
