using System.Collections.Generic;
using LifxMvc.Domain;

namespace LifxMvc.Services
{
	public interface IDiscoveryService
	{
		List<IBulb> DiscoverAsync(int expectedCount);
	}
}