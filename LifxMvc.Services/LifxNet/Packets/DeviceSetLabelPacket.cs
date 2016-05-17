using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceSetLabelPacket : LifxPacketBase<DeviceAcknowledgementResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceSetLabel; } }

		const int MAX_LENGTH = 32;
		string _label;
		public string Label
		{
			get { return _label ?? string.Empty; }
			set
			{
				if (null == value)
					value = string.Empty;
				if (MAX_LENGTH < value.Length)
					throw new ArgumentOutOfRangeException("Maximum Label length is 32.");
				_label = value;
			}
		}
		public DeviceSetLabelPacket(Bulb bulb, string label) : base(bulb)
		{
			this.Header.AcknowledgeRequired = true;
			this.Label = label;
		}

		override protected object[] GetPayloadParams()
		{
			_label = _label.PadRight(MAX_LENGTH);
			return new object[] { _label };
		}
	}
}
