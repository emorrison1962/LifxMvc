using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceEchoRequestPacket : LifxPacketBase<DeviceEchoResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceEchoRequest; } }
		public DeviceEchoRequestPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
		override protected object[] GetPayloadParams()
		{
			var payload = new byte[64];
			return new object[] { payload };
		}

	}
}
