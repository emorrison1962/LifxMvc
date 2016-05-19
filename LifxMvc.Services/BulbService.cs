using LifxMvc.Domain;
using LifxMvc.Services;
using LifxMvc.Services.UdpHelper;
using LifxNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LifxMvc.Services
{
	public class BulbService
	{
		R Send<R>(Bulb bulb, LifxPacketBase<R> packet) where R : LifxResponseBase
		{
			var udp = UdpHelperManager.Instance[packet.IPEndPoint];
			var response = udp.Send(packet);
			if (null != response)
				BulbExtensions.Set(bulb, (dynamic)response);
			return response;
		}

		void SendAsync(Bulb bulb, LifxPacketBase packet) 
		{
			var udp = UdpHelperManager.Instance[packet.IPEndPoint];
			udp.SendAsync(packet);
		}

		public void LightGet(Bulb bulb)
		{
			var packet = new LightGetPacket(bulb);
			this.Send(bulb, packet);
		}

		public bool DeviceGetPower(Bulb bulb)
		{
			var packet = new DeviceGetPowerPacket(bulb);
			var response = this.Send(bulb, packet);

			var result = response.IsOn;
			return result;
		}

		public void DeviceSetPower(Bulb bulb, bool isOn)
		{
			var packet = new DeviceSetPowerPacket(bulb, isOn);
			this.Send(bulb, packet);

			bulb.IsOn = isOn;
		}

		public void DeviceGetVersion(Bulb bulb)
		{
			var packet = new DeviceGetVersionPacket(bulb);
			this.Send(bulb, packet);
		}

		
		public void LightSetColor(Bulb bulb, HSBK hsbk)
		{
			var packet = new LightSetColorPacket(bulb, hsbk);
			packet.Duration = 100;
			this.SendAsync(bulb, packet);

			bulb.SetColor(hsbk);
		}

		public void GetHostInfo(Bulb bulb)
		{
			var packet = new DeviceGetHostInfoPacket(bulb);
			this.Send(bulb, packet);
		}
		public void GetHostFirmware(Bulb bulb)
		{
			var packet = new DeviceGetHostFirmwarePacket(bulb);
			this.Send(bulb, packet);
		}
		public void GetWifiInfo(Bulb bulb)
		{
			var packet = new DeviceGetWifiInfoPacket(bulb);
			this.Send(bulb, packet);
		}
		public void GetWifiFirmware(Bulb bulb)
		{
			var packet = new DeviceGetWifiFirmwarePacket(bulb);
			this.Send(bulb, packet);
		}
		public void GetLabel(Bulb bulb)
		{
			var packet = new DeviceGetLabelPacket(bulb);
			this.Send(bulb, packet);
		}
		public void SetLabel(Bulb bulb, string label)
		{
			var packet = new DeviceSetLabelPacket(bulb, label);
			this.Send(bulb, packet);
		}

		public void GetGroup(Bulb bulb)
		{
			var packet = new DeviceGetGroupPacket(bulb);
			this.Send(bulb, packet);
		}
		public void GetLocation(Bulb bulb)
		{
			var packet = new DeviceGetLocationPacket(bulb);
			this.Send(bulb, packet);
		}

		public void GetInfo(Bulb bulb)
		{
			var packet = new DeviceGetInfoPacket(bulb);
			this.Send(bulb, packet);
		}
		public void EchoRequest(Bulb bulb)
		{
			var packet = new DeviceEchoRequestPacket(bulb);
			this.Send(bulb, packet);
		}
		public bool LightGetPower(Bulb bulb)
		{
			var packet = new LightGetPowerPacket(bulb);
			this.Send(bulb, packet);
			return bulb.IsOn;
		}
		public void LightSetPower(Bulb bulb, bool power)
		{
			var packet = new LightSetPowerPacket(bulb, power);
			this.Send(bulb, packet);
		}



	}//class

	public static class BulbExtensions
	{
		public static void SetColor(this Bulb bulb, HSBK hsbk)
		{
			bulb.Hue = hsbk.Hue;
			bulb.Saturation = hsbk.Saturation;
			bulb.Brightness = hsbk.Brightness;
			bulb.Kelvin = hsbk.Kelvin;
		}

		public static void Set(this Bulb bulb, DeviceAcknowledgementResponse r)
		{
		}

		public static void Set(this Bulb bulb, DeviceEchoResponse r)
		{
		}

		public static void Set(this Bulb bulb, DeviceStateGroupResponse r)
		{
			bulb.Group = r.Label;
		}

		public static void Set(this Bulb bulb, DeviceStateHostFirmwareResponse r)
		{
			bulb.HostFirmwareBuild = r.Build;
			bulb.HostFirmwareVersion = r.Version;
		}

		public static void Set(this Bulb bulb, DeviceStateHostInfoResponse r)
		{
			bulb.Signal = r.Signal;
			bulb.TxCount = r.TxCount;
			bulb.RxCount = r.RxCount;
		}

		public static void Set(this Bulb bulb, DeviceStateInfoResponse r)
		{
			bulb.Time = r.Time;
			bulb.Uptime = r.Uptime;
			bulb.Downtime = r.Downtime;
		}

		public static void Set(this Bulb bulb, DeviceStateLabelResponse r)
		{
			bulb.Label = r.Label;
		}
		
		public static void Set(this Bulb bulb, DeviceStateLocationResponse r)
		{
			bulb.Location = r.Label;
		}

		public static void Set(this Bulb bulb, DeviceStatePowerResponse r)
		{
			bulb.IsOn = r.IsOn;
		}

		
		public static void Set(this Bulb bulb, DeviceStateServiceResponse r)
		{
			bulb.Service = r.Service;
			bulb.Port = r.Port;
		}
		
		public static void Set(this Bulb bulb, DeviceStateVersionResponse r)
		{
			bulb.Vendor = r.Vendor;
			bulb.Product = r.Product;
			bulb.Version = r.Version;
		}

		public static void Set(this Bulb bulb, DeviceStateWifiFirmwareResponse r)
		{
			bulb.WifiFirmwareBuild = r.Build;
			bulb.WifiFirmwareVersion = r.Version;
		}

		public static void Set(this Bulb bulb, DeviceStateWifiInfoResponse r)
		{
			bulb.WifiInfoSignal = r.Signal;
			bulb.WifiInfoTxCount = r.TxCount;
			bulb.WifiInfoRxCount = r.RxCount;
		}

		
		public static void Set(this Bulb bulb, LightStatePowerResponse r)
		{
			bulb.IsOn = r.IsOn;
		}

		public static void Set(this Bulb bulb, LightStateResponse r)
		{
			bulb.Hue = r.Hue;
			bulb.Saturation = r.Saturation;
			bulb.Brightness = r.Brightness;
			bulb.Kelvin = r.Kelvin;
			bulb.IsOn = r.IsOn;
			bulb.Label = r.Label;
		}

		public static void Set(this Bulb bulb, UnknownResponse r)
		{
			throw new ArgumentOutOfRangeException();
		}
		



	}//class

}//ns
