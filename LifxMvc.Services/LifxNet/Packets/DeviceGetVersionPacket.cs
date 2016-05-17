using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetVersionPacket : LifxPacketBase<DeviceStateVersionResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceGetVersion; } }

		public DeviceGetVersionPacket(Bulb bulb)
			: base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}//class
}
