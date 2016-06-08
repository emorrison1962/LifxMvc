using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace LifxMvc.Models
{
	public class BulbsViewModel
	{
		public List<GroupViewModel> Groups { get; private set; }

		public BulbsViewModel()
		{
			this.Groups = new List<GroupViewModel>();
		}
		public BulbsViewModel(IEnumerable<Bulb> bulbs) : this()
		{
			var groupNames = (
				from bulb in bulbs
				select bulb.Group).Distinct();

			foreach (var groupName in groupNames)
			{
				var groupBulbs = bulbs.Where(x => x.Group == groupName).ToList();
				this.Groups.Add(new GroupViewModel(groupName, groupBulbs));
			}

		}
	}

	public class GroupViewModel
	{
		public string Name { get; private set; }
		public List<BulbViewModel> Bulbs { get; private set; }

		public GroupViewModel()
		{
			this.Bulbs = new List<BulbViewModel>();

		}
		public GroupViewModel(string name, List<Bulb> bulbs) : this()
		{
			this.Name = name;
			foreach (var bulb in bulbs)
			{
				this.Bulbs.Add(new BulbViewModel(bulb));
			}

			this.Bulbs.OrderBy(x => x.Label);
		}
	}

	public class BulbViewModel : IBulb
	{
		public int BulbId { get; private set; }
		public Color Color { get; set; }
		public string Group { get; set; }
		public UInt16 Hue { get; set; }
		public UInt16 Saturation { get; set; }
		public UInt16 Brightness { get; set; }
		public UInt16 Kelvin { get; set; }
		public bool IsOn { get; set; }
		public string Label { get; set; }
		public string Location { get; set; }
		public Single Signal { get; set; }
		public UInt32 TxCount { get; set; }
		public UInt32 RxCount { get; set; }
		public Single WifiInfoSignal { get; set; }
		public UInt32 WifiInfoTxCount { get; set; }
		public UInt32 WifiInfoRxCount { get; set; }
		public DateTime Time { get; set; }
		public DateTime Uptime { get; set; }
		public DateTime Downtime { get; set; }
		public DateTime Build { get; set; }
		public UInt32 WifiFirmwareVersion { get; set; }
		public DateTime WifiFirmwareBuild { get; set; }
		public UInt32 HostFirmwareVersion { get; set; }
		public DateTime HostFirmwareBuild { get; set; }
		public string IPAddress { get; set; }
		public byte[] TargetMacAddress { get; set; }
		public byte Service { get; set; }
		public uint Port { get; set; }
		public DateTime LastSeen { get; set; }
		public uint Vendor { get; set; }
		public uint Product { get; set; }
		public uint Version { get; set; }

		public BulbViewModel(Bulb bulb)
		{
			this.BulbId = bulb.BulbId;
			this.Brightness = bulb.Brightness;
			this.Build = bulb.Build;
			this.Color = bulb.Color;
			this.Downtime = bulb.Downtime;
			this.Group = bulb.Group;
			this.HostFirmwareBuild = bulb.HostFirmwareBuild;
			this.HostFirmwareVersion = bulb.HostFirmwareVersion;
			this.Hue = bulb.Hue;
			this.IsOn = bulb.IsOn;
			this.Kelvin = bulb.Kelvin;
			this.Label = bulb.Label;
			this.LastSeen = bulb.LastSeen;
			this.Location = bulb.Location;
			this.Port = bulb.Port;
			this.Product = bulb.Product;
			this.RxCount = bulb.RxCount;
			this.Saturation = bulb.Saturation;
			this.Service = bulb.Service;
			this.Signal = bulb.Signal;
			this.TargetMacAddress = bulb.TargetMacAddress;
			this.Time = bulb.Time;
			uint TxCount = bulb.TxCount;
			this.Uptime = bulb.Uptime;
			this.Vendor = bulb.Vendor;
			this.Version = bulb.Version;
			this.WifiFirmwareBuild = bulb.WifiFirmwareBuild;
			this.WifiFirmwareVersion = bulb.WifiFirmwareVersion;
			this.WifiInfoRxCount = bulb.WifiInfoRxCount;
			this.WifiInfoSignal = bulb.WifiInfoSignal;
			this.WifiInfoTxCount = bulb.WifiInfoTxCount;
			this.IPAddress = bulb.IPEndPoint.Address.ToString();
		}
	}


}