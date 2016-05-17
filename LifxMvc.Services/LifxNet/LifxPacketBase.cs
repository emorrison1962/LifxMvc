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
		public abstract RequestType MessageType { get; }

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

		public byte[] SerializeMessage()
		{
			object[] args = this.GetPayloadParams();
			List<byte> payload = new List<byte>();
			if (args != null)
			{
				foreach (var arg in args)
				{
					if (arg is UInt16)
						payload.AddRange(BitConverter.GetBytes((UInt16)arg));
					else if (arg is UInt32)
						payload.AddRange(BitConverter.GetBytes((UInt32)arg));
					else if (arg is byte)
						payload.Add((byte)arg);
					else if (arg is byte[])
						payload.AddRange((byte[])arg);
					else if (arg is string)
						payload.AddRange(Encoding.UTF8.GetBytes(((string)arg).PadRight(32).Take(32).ToArray())); //All strings are 32 bytes
					else
						throw new NotSupportedException(args.GetType().FullName);
				}
			}

			byte[] result = SerializeMessagePayload(payload.ToArray());
			return result;
		}
		protected byte[] SerializeMessagePayload(byte[] payload)
		{
			MemoryStream ms = new MemoryStream();
			WritePacketToStream(ms, payload);
			var result = ms.ToArray();

#if false
			System.Diagnostics.Debug.WriteLine(
				string.Join(",", (from a in result select a.ToString("X2")).ToArray()));

#endif
			return result;
		}
		protected void WritePacketToStream(Stream stream, byte[] payload)
		{
			using (BinaryWriter writer = new BinaryWriter(stream))
			{
				//BinaryWriter bw = new BinaryWriter(ms);

				this.Header.WriteToStream(writer, payload);

				writer.Write((UInt16)this.MessageType); //packet _type
				writer.Write((UInt16)0); //reserved
				if (payload != null)
					writer.Write(payload);

				//writer.StoreAsync();
			}
		}

		virtual protected object[] GetPayloadParams()
		{
			return null;
		}




	}//class












	







}//ns