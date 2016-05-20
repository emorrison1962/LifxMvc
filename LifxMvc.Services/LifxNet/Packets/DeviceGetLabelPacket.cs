using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetLabelPacket : LifxPacketBase<DeviceStateLabelResponse>
	{
		override public PacketType MessageType { get { return PacketType.DeviceGetLabel; } }
		public DeviceGetLabelPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}

	}
}
