using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceSetPowerPacket : LifxPacketBase<DeviceAcknowledgementResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceSetPower; } }
		public bool IsOn { get; private set; }
		public DeviceSetPowerPacket(Bulb bulb, bool isOn)
			: base(bulb)

		{

			this.IsOn = isOn;
			this.Header.AcknowledgeRequired = true;
		}

		override protected object[] GetPayloadParams()
		{
			UInt16 level = this.IsOn ? UInt16.MaxValue : (UInt16)0U;
			return new object[] { level };
		}
	}
}
