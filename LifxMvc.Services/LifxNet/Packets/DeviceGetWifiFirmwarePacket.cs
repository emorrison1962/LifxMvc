using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetWifiFirmwarePacket : LifxPacketBase<DeviceStateWifiFirmwareResponse>
	{
		override public PacketType MessageType { get { return PacketType.DeviceGetWifiFirmware; } }
		public DeviceGetWifiFirmwarePacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}
}
