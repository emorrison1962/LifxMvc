using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	/// <summary>
	/// Base class for LIFX response types
	/// </summary>
	public abstract class LifxResponseBase
	{
		public static LifxResponseBase Parse(byte[] packet, IPEndPoint sender)
		{
			using (MemoryStream ms = new MemoryStream(packet))
			{
				var header = new FrameHeader();
				BinaryReader br = new BinaryReader(ms);
				//frame
				var size = br.ReadUInt16();
				if (packet.Length != size || size < 36)
					throw new Exception("Invalid packet");
				var a = br.ReadUInt16(); //origin:2, reserved:1, addressable:1, protocol:12
				var source = br.ReadUInt32();
				header.Source = source;
				//frame address
				byte[] target = br.ReadBytes(8);
				header.TargetMacAddress = target;
				ms.Seek(6, SeekOrigin.Current); //skip reserved
				var b = br.ReadByte(); //reserved:6, ack_required:1, res_required:1, 
				header.Sequence = br.ReadByte();

				//protocol header
				var nanoseconds = br.ReadUInt64();
				header.AtTime = Constants.Epoch.AddMilliseconds(nanoseconds * 0.000001);
				var type = (ResponseType)br.ReadUInt16();
				ms.Seek(2, SeekOrigin.Current); //skip reserved


				byte[] payload = null;
				if (size > 36)
					payload = br.ReadBytes(size - 36);
				return LifxResponseBase.Create(header, type, source, payload, sender);
			}
		}

		public static LifxResponseBase Create(FrameHeader header, ResponseType type, UInt32 source, byte[] payload, IPEndPoint sender)
		{
			LifxResponseBase response = null;
			switch (type)
			{
				case ResponseType.DeviceAcknowledgement:
					response = new DeviceAcknowledgementResponse(payload);
					break;
				case ResponseType.DeviceStateLabel:
					response = new DeviceStateLabelResponse(payload);
					break;
				case ResponseType.LightState:
					response = new LightStateResponse(payload);
					break;
				case ResponseType.LightStatePower:
					response = new LightStatePowerResponse(payload);
					break;
				case ResponseType.DeviceStateVersion:
					response = new DeviceStateVersionResponse(payload);
					break;
				case ResponseType.DeviceStateHostFirmware:
					response = new DeviceStateHostFirmwareResponse(payload);
					break;
				case ResponseType.DeviceStateService:
					response = new DeviceStateServiceResponse(header, payload);
					break;

				case ResponseType.DeviceStatePower:
					response = new DeviceStatePowerResponse(payload);
					break;

				case ResponseType.DeviceEcho:
					response = new DeviceEchoResponse(payload);
					break;

				case ResponseType.DeviceStateHostInfo:
					response = new DeviceStateHostInfoResponse(payload);
					break;
				case ResponseType.DeviceStateWifiInfo:
					response = new DeviceStateWifiInfoResponse(payload);
					break;

				case ResponseType.DeviceStateTime:
					response = new DeviceStateTimeResponse(payload);
					break;

				case ResponseType.DeviceStateWifiFirmware:
					response = new DeviceStateWifiFirmwareResponse(payload);
					break;

				case ResponseType.DeviceStateInfo:
					response = new DeviceStateInfoResponse(payload);
					break;

				case ResponseType.DeviceStateLocation:
					response = new DeviceStateLocationResponse(payload);
					break;


				case ResponseType.DeviceStateGroup:
					response = new DeviceStateGroupResponse(payload);
					break;



				default:
					response = new UnknownResponse(payload);
					break;
			}
			response.Header = header;
			response.Type = type;
			response.Payload = payload;
			response.Source = source;
			response.IPEndPoint = sender;
			return response;
		}

		public LifxResponseBase() { }
		public FrameHeader Header { get; protected set; }
		public byte[] Payload { get; protected set; }
		public ResponseType Type { get; private set; }
		public UInt32 Source { get; private set; }
		public IPEndPoint IPEndPoint { get; set; }

		public override string ToString()
		{
			var result = string.Format("Source={1}, Sequence={2} : {0}",
					base.ToString(), 
					this.Header.Source.ToString("X8"), 
					this.Header.Sequence.ToString("X2"));
			return result;
		}
	}

















	
}//ns
