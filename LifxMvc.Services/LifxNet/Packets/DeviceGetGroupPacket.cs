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
		override public PacketType MessageType { get { return PacketType.DeviceGetGroup; } }
		public DeviceGetGroupPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}
}
