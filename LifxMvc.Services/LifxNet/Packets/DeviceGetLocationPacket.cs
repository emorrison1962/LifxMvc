﻿using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class DeviceGetLocationPacket : LifxPacketBase<DeviceStateLocationResponse>
	{
		override public RequestType MessageType { get { return RequestType.DeviceGetLocation; } }
		public DeviceGetLocationPacket(Bulb bulb) : base(bulb)
		{
			this.Header.ResponseRequired = true;
		}
	}
}