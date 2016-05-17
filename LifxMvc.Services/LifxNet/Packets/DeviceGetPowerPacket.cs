﻿using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetPowerPacket : LifxPacketBase<DeviceStatePowerResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceGetPower; } }
		public DeviceGetPowerPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}
}
