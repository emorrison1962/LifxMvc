using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class LightGetPacket : LifxPacketBase<LightStateResponse>
	{
		override public RequestType MessageType { get { return RequestType.LightGet; } }
		public LightGetPacket(Bulb bulb)
			: base(bulb)
		{
			
		}
	}
}
