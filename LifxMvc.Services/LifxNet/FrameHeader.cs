using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public class FrameHeader
	{
		static UInt32 _nextIdentifier = 1;
		UInt32 _identifier = ++_nextIdentifier;

		public UInt32 Source;
		public bool Tagged;
		public byte Sequence;
		public bool AcknowledgeRequired;
		public bool ResponseRequired;
		public byte[] TargetMacAddress;
		public DateTime AtTime;
		public FrameHeader(bool acknowledgeRequired = false)
		{
			if (0 == _identifier)
				++_identifier;
			Source = _identifier;
			Sequence = 0;
			AcknowledgeRequired = acknowledgeRequired;
			ResponseRequired = false;
			TargetMacAddress = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
			AtTime = DateTime.MinValue;
			this.Tagged = false;

		}

		public void WriteToStream(BinaryWriter writer, byte[] payload)
		{
			#region Frame
			//size uint16
			writer.Write((UInt16)((payload != null ? payload.Length : 0) + 36)); //length

#if true
			UInt16 protocol = 0x1400;
			const UInt16 tagged = 1 << 13; //Account for littleEndian
			if (this.Tagged)
				protocol |= tagged;
			writer.Write((UInt16)protocol); //protocol

#else
				// origin (2 bits, must be 0), 
				// tagged (1 bit), Determines usage of the Frame Address target field.
				//The tagged field is a boolean flag that indicates whether the Frame Address target field is being used to address an individual device or all devices. For discovery using Device::GetService the tagged field should be set to one (1) and the target should be all zeroes. In all other messages the tagged field should be set to zero (0) and the target field should contain the device MAC address. The device will then respond with a Device::StateService message, which will include its own MAC address in the target field. In all subsequent messages that the client sends to the device, the target field should be set to the device MAC address, and the tagged field should be set to zero (0).

				// addressable (1 bit, must be 1), 
				//protocol 12 bits must be 0x400) = 0x1400
				writer.Write((UInt16)0x3400); //protocol
				//writer.Write((UInt16)0x1400); //protocol

#endif


			writer.Write((UInt32)this.Source); //source identifier - unique value set by the client, used by responses. If 0, responses are broadcasted instead
			Debug.Assert(0 != this.Source);

			#endregion Frame

			#region Frame address
			//The target device address is 8 bytes long, when using the 6 byte MAC address then left - 
			//justify the value and zero-fill the last two bytes. A target device address of all zeroes effectively addresses all devices on the local network
			writer.Write(this.TargetMacAddress); // target mac address - 0 means all devices
			writer.Write(new byte[] { 0, 0, 0, 0, 0, 0 }); //reserved 1

			//The client can use acknowledgements to determine that the LIFX device has received a message. 
			//However, when using acknowledgements to ensure reliability in an over-burdened lossy network ... 
			//causing additional network packets may make the problem worse. 
			//Client that don't need to track the updated state of a LIFX device can choose not to request a 
			//response, which will reduce the network burden and may provide some performance advantage. In
			//some cases, a device may choose to send a state update response independent of whether res_required is set.
			if (this.AcknowledgeRequired && this.ResponseRequired)
				writer.Write((byte)0x03);
			else if (this.AcknowledgeRequired)
				writer.Write((byte)0x02);
			else if (this.ResponseRequired)
				writer.Write((byte)0x01);
			else
				writer.Write((byte)0x00);
			//The sequence number allows the client to provide a unique value, which will be included by the LIFX 
			//device in any message that is sent in response to a message sent by the client. This allows the client
			//to distinguish between different messages sent with the same source identifier in the Frame. See
			//ack_required and res_required fields in the Frame Address.
			writer.Write(this.Sequence);
			#endregion Frame address

			#region Protocol Header
			//The at_time value should be zero for Set and Get messages sent by a client.
			//For State messages sent by a device, the at_time will either be the device
			//current time when the message was received or zero. StateColor is an example
			//of a message that will return a non-zero at_time value
			if (this.AtTime > DateTime.MinValue)
			{
				var time = this.AtTime.ToUniversalTime();
				writer.Write((UInt64)(time - new DateTime(1970, 01, 01)).TotalMilliseconds * 10); //timestamp
			}
			else
			{
				writer.Write((UInt64)0);
			}
			#endregion Protocol Header


		}

	}
}
