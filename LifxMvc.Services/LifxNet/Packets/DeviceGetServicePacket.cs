using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetServicePacket : LifxPacketBase<DeviceStateServiceResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceGetService; } }

		public DeviceGetServicePacket()
			: base(null)
		{
			this.Header.AcknowledgeRequired = false;
			this.Header.Tagged = true;
			this.Header.Source = UInt32.MaxValue;
		}
	}
}
