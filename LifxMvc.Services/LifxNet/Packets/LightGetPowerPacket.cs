using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class LightGetPowerPacket : LifxPacketBase<LightStatePowerResponse>
	{
		override public RequestType MessageType { get { return RequestType.LightGetPower; } }
		public LightGetPowerPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}
}
