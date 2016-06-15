using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifxMvc.Domain;
using LifxMvc.Domain.Mocks;

namespace LifxMvc.Services
{
	public class DiscoveryServiceMock : IDiscoveryService
	{
		public List<IBulb> DiscoverAsync(int expectedCount)
		{
			return CreateBulbs();
		}

		List<IBulb> CreateBulbs()
		{
			var result = new List<IBulb>();
			for (int groupNo = 1; groupNo < 5; groupNo++)
			{
				for (int bulbNo = 1; bulbNo < 3; bulbNo++)
				{
					var bulb = new BulbMock();
					bulb.Brightness = 100;
					bulb.Hue = 100;
					bulb.Saturation = 100;
					bulb.Kelvin = 100;
					bulb.Label = string.Format("Bulb{0}", bulbNo);
					bulb.Group = string.Format("Group{0}", groupNo);
					bulb.IsOn = false;
					result.Add(bulb);
				}
			}


			return result;
		}
	}
}
