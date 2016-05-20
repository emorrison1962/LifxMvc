using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class LightSetColorPacket : LifxPacketBase<LightStateResponse>
	{
		override public PacketType MessageType { get { return PacketType.LightSetColor; } }

		public UInt16 Hue { get; private set; }
		public UInt16 Saturation { get; private set; }
		public UInt16 Brightness { get; private set; }
		public UInt16 Kelvin { get; private set; }

		public UInt32 Duration { get; set; }

		public LightSetColorPacket(Bulb bulb, HSBK hsbk)
			: base(bulb)
		{


			this.Hue = hsbk.Hue;
			this.Saturation = hsbk.Saturation;
			this.Brightness = hsbk.Brightness;
			this.Kelvin = hsbk.Kelvin;

			this.Header.ResponseRequired = true;
		}

		override protected object[] GetPayloadParams()
		{
			return new object[] { (byte)0, this.Hue, this.Saturation, this.Brightness, this.Kelvin, this.Duration };
		}
	}
}
