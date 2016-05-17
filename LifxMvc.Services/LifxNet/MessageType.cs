using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public enum RequestType : ushort
	{
		//Device requests
		DeviceGetService = 0x02,
		DeviceGetHostInfo = 12,
		DeviceGetHostFirmware = 14,
		DeviceGetWifiInfo = 16,
		DeviceGetWifiFirmware = 18,
		DeviceGetPower = 20,
		DeviceSetPower = 21,
		DeviceGetLabel = 23,
		DeviceSetLabel = 24,
		DeviceGetVersion = 32,
		DeviceGetInfo = 34,
		DeviceGetLocation = 48,
		DeviceGetGroup = 51,
		DeviceEchoRequest = 58,

		//Light messages
		LightGet = 101,
		LightSetColor = 102,
		LightGetPower = 116,
		LightSetPower = 117,


		//Unofficial
		LightGetTemperature = 0x6E,
		//LightStateTemperature = 0x6f,
		SetLightBrightness = 0x68,

		Unknown = ushort.MaxValue
	}

	public enum ResponseType : ushort
	{
		//Device responses
		DeviceStateService = 0x03,
		DeviceStateTime = 0x06,
		DeviceStateHostInfo = 13,
		DeviceStateHostFirmware = 15,
		DeviceStateWifiInfo = 17,
		DeviceStateWifiFirmware = 19,
		DeviceStatePower = 22,
		DeviceStateLabel = 25,
		DeviceStateVersion = 33,
		DeviceStateInfo = 35,
		DeviceAcknowledgement = 45,
		DeviceStateLocation = 50,
		DeviceStateGroup = 53,
		DeviceEcho = 59,
		LightState = 107,
		LightStatePower = 118,

	}




}//ns
