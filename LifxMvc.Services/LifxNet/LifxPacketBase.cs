using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LifxNet
{
	public abstract class LifxPacketBase<R> : LifxPacketBase where R : LifxResponseBase
	{
		public Type ExpectedResponseType { get { return typeof(R); } }
		public LifxPacketBase(Bulb bulb)
			: base(bulb)
			{}
	}

	public abstract class LifxPacketBase
	{
		#region Fields

		private byte[] _payload;
		private ushort _type;

		#endregion

		#region Properties

		public FrameHeader Header { get; protected set; }
		public IPEndPoint IPEndPoint { get; set; }
		public abstract PacketType MessageType { get; }

		/// <summary>
		/// ///////////////////////////////////////////////////////////
		/// </summary>
		public byte[] Payload { get { return _payload; } }
		public ushort Type { get { return (ushort)this.MessageType; } }


		#endregion        
		
		#region Construction

		protected LifxPacketBase(Bulb bulb)
		{
			this.Header = new FrameHeader();
			if (null != bulb)
			{
				this.IPEndPoint = bulb.IPEndPoint;
				this.Header.TargetMacAddress = bulb.TargetMacAddress;
			}
		}
		#endregion

		public byte[] Serialize()
		{
			List<byte> bytes = new List<byte>();

			var payload = this.GetPayloadBytes();
			UInt16 payloadLength = (UInt16)payload.Length;
			var header = this.Header.GetBytes(payloadLength, this.MessageType);

			using (var ms = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(ms))
				{
					writer.Write(header);
					if (payload != null)
					{
						writer.Write(payload);
					}
					bytes.AddRange(ms.ToArray());
				}
			}

			var result = bytes.ToArray();
			return result;
		}

		public byte[] GetPayloadBytes()
		{
			object[] args = this.GetPayloadParams();
			List<byte> bytes = new List<byte>();

			if (args != null)
			{
				foreach (var arg in args)
				{
					if (arg is byte)
						bytes.Add((byte)arg);
					else if (arg is byte[])
						bytes.AddRange((byte[])arg);
					else if (arg is string)
					{
						var chars = ((string)arg).PadRight(32, (char)0).Take(32).ToArray();
						var charBytes = Encoding.UTF8.GetBytes(chars);
						bytes.AddRange(charBytes); //All strings are 32 bytes
					}
					else
					{
						bytes.AddRange(BitConverter.GetBytes((dynamic)arg));
					}
				}
			}

			byte[] result = bytes.ToArray();
			return result;
		}


		virtual protected object[] GetPayloadParams()
		{
			return null;
		}

		public override string ToString()
		{
			var result = string.Format("Source={1}, Sequence={2} : {0}",
					base.ToString(),
					this.Header.Source.ToString("X8"),
					this.Header.Sequence.ToString("X2"));
			return result;
		}

	}//class




















}//ns