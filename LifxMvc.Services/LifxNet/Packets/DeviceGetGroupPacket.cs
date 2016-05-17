using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetGroupPacket : LifxPacketBase<DeviceStateGroupResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceGetGroup; } }
		public DeviceGetGroupPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}
}
