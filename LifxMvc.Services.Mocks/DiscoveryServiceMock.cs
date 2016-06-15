using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifxMvc.Domain;

namespace LifxMvc.Services
{
	public class DiscoveryServiceMock : IDiscoveryService
	{
		public List<IBulb> DiscoverAsync(int expectedCount)
		{
			throw new NotImplementedException();
		}
	}
}
