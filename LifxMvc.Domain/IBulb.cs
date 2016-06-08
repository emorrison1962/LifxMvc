using System;
using System.Drawing;

namespace LifxMvc.Domain
{
	public interface IBulb
	{
		ushort Brightness { get; set; }
		DateTime Build { get; set; }
		int BulbId { get; }
		Color Color { get; set; }
		DateTime Downtime { get; set; }
		string Group { get; set; }
		DateTime HostFirmwareBuild { get; set; }
		uint HostFirmwareVersion { get; set; }
		ushort Hue { get; set; }
		bool IsOn { get; set; }
		ushort Kelvin { get; set; }
		string Label { get; set; }
		DateTime LastSeen { get; set; }
		string Location { get; set; }
		uint Port { get; set; }
		uint Product { get; set; }
		uint RxCount { get; set; }
		ushort Saturation { get; set; }
		byte Service { get; set; }
		float Signal { get; set; }
		byte[] TargetMacAddress { get; set; }
		DateTime Time { get; set; }
		uint TxCount { get; set; }
		DateTime Uptime { get; set; }
		uint Vendor { get; set; }
		uint Version { get; set; }
		DateTime WifiFirmwareBuild { get; set; }
		uint WifiFirmwareVersion { get; set; }
		uint WifiInfoRxCount { get; set; }
		float WifiInfoSignal { get; set; }
		uint WifiInfoTxCount { get; set; }
	}
}