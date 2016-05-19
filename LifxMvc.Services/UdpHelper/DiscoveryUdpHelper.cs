using LifxNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LifxMvc.Services.UdpHelper
{
	public class DiscoveryUdpHelper : IDisposable
	{
		private const int PORT_NO = 56700;

		Task ListeningTask { get; set; }
		ManualResetEventSlim StopListeningEvent { get; set; }

		public class DiscoveryEventArgs : EventArgs
		{
			public DiscoveryContext DiscoveryContext { get; private set; }
			public DiscoveryEventArgs(DiscoveryContext discoveryContext)
			{
				this.DiscoveryContext = discoveryContext;
			}
		}
		public event EventHandler<DiscoveryEventArgs> DeviceDiscovered;

		public DiscoveryUdpHelper()
		{
			this.StopListeningEvent = new ManualResetEventSlim(false);
		}

		bool _discoveryComplete = false;
		public void BroadcastAndListen(LifxPacketBase packet, Action<DiscoveryContext> listenCallback, int expectedCount, int timeout)
		{
			try
			{
				var data = packet.SerializeMessage();
				TraceData(data);
				IPAddress broadcastIP = IPAddress.Broadcast;
				IPEndPoint destEP = new IPEndPoint(broadcastIP, PORT_NO);
				packet.IPEndPoint = destEP;
				using (UdpClient sender = new UdpClient(PORT_NO, AddressFamily.InterNetwork))
				{
					sender.DontFragment = true;
					sender.EnableBroadcast = true;

					//var resendWait = new ManualResetEventSlim(false);
					sender.Send(data, data.Length, destEP);
					packet.TraceSent(sender.Client.LocalEndPoint);
					//resendWait.Wait(100);

					//sender.Send(data, data.Length, destEP);
					////resendWait.Wait(100);

					//sender.Send(data, data.Length, destEP);
					////resendWait.Wait(100);

				}
				this.StartListening(listenCallback, expectedCount, timeout);
				var wait = new ManualResetEventSlim(false);
				wait.Wait(timeout);
#warning FIXME:
				//this.StopListening();
				_discoveryComplete = true;

			}
			catch (Exception ex)
			{
				Debug.WriteLine(" Exception {0}", ex.Message);
				throw;
			}
		}

		public void StartListening(Action<DiscoveryContext> listenCallback, int expectedCount, int timeout)
		{
			this.StopListeningEvent.Reset();
			//var action = new Action(delegate()
			//{
			//	this.Listen(listenCallback, expectedCount, timeout);
			//});
			//this.ListeningTask = new Task(action);
			//this.ListeningTask.Start();

			var ctx = new ListenContext(listenCallback, expectedCount, timeout);
			var listenProc = new Thread(this.Listen);
			listenProc.Start(ctx);
		}

		internal void StopListening()
		{
			this.StopListeningEvent.Set();
		}

		class ListenContext
		{
			public Action<DiscoveryContext> ListenCallback { get; set; }
			public int ExpectedCount { get; set; }
			public int Timeout { get; set; }

			public ListenContext(Action<DiscoveryContext> listenCallback, int expectedCount, int timeout)
			{
				this.ListenCallback = listenCallback;
				this.ExpectedCount = expectedCount;
				this.Timeout = timeout;
			}
		}
		public void Listen(object ob)
		{
			var listenCtx = ob as ListenContext;
			int timeout = listenCtx.Timeout;
			int expectedCount = listenCtx.ExpectedCount;
			var listenCallback = listenCtx.ListenCallback;
			_discoveryComplete = false;



			IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

			using (UdpClient listener = new UdpClient(PORT_NO, AddressFamily.InterNetwork))
			{
				listener.EnableBroadcast = true;
				try
				{
					while (!this.StopListeningEvent.IsSet)
					{
						var asyncResult = listener.BeginReceive(null, null);
						var waitHandles = new WaitHandle[] { asyncResult.AsyncWaitHandle, this.StopListeningEvent.WaitHandle };
						var which = int.MaxValue;
						try
						{
							which = WaitHandle.WaitAny(waitHandles, timeout);
						}
						catch (ThreadAbortException)
						{ }
						if (0 == which)
						{
							if (asyncResult.IsCompleted)
							{
								byte[] data = listener.EndReceive(asyncResult, ref sender);
								//parse the response.
								var response = LifxResponseBase.Parse(data, sender);
								
								if (_discoveryComplete)
								{
									//This is a random response broadcast from a bulb.
									//Debug.WriteLine(string.Format("{0}: {1}", sender.ToString(), response.ToString()));
								}
								response.TraceReceived(listener.Client.LocalEndPoint, _discoveryComplete);

								if (response is DeviceStateServiceResponse)
								{
									//publish the response.
									var ctx = new DiscoveryContext(sender, response as DeviceStateServiceResponse, expectedCount);
									if (null != this.DeviceDiscovered)
									{
										this.DeviceDiscovered(this, new DiscoveryEventArgs(ctx));
										if (ctx.CancelDiscovery)
											_discoveryComplete = true;
									}

//									if (null != listenCallback)
//									{
//										var ctx = new DiscoveryContext(sender, response as DeviceStateServiceResponse, expectedCount);
//										if (null != this.DeviceDiscovered)
//											this.DeviceDiscovered(this, new DiscoveryEventArgs(ctx));

//										try
//										{
//											listenCallback(ctx);
//										}
//										catch(Exception ex)
//										{ }
//#warning FIXME:
//										//if (ctx.CancelDiscovery)
//										//	this.StopListening();
//									}
								}
							}
						}
						else if (1 == which)
						{
							listener.Close();
							this.StopListening();
						}
					}
					Debug.WriteLine("StopListeningEvent.IsSet");
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.ToString());
					throw;
				}
			}
		}


		UdpClient CreateUdpClient()
		{
			var client = new UdpClient(PORT_NO);
			return client;
		}

		public class DiscoveryCallbackContext
		{
			public Action<object, byte[], IPEndPoint> Callback { get; private set; }
		}


		static void TraceData(byte[] data)
		{
#if false
			if (null != data)
			{
				System.Diagnostics.Debug.WriteLine(
					string.Join(",", (from a in data select Convert.ToString(a, 2).PadLeft(8, '0')).ToArray()));

				//System.Diagnostics.Debug.WriteLine(
				//	string.Join(",", (from a in data select a.ToString("X2")).ToArray()));
			}

#endif
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this.StopListening();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~DiscoveryUdpHelper() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}//class 

	public class DiscoveryContext
	{
		public int ExpectedCount { get; set; }
		public IPEndPoint Sender { get; private set; }
		public DeviceStateServiceResponse Response { get; private set; }
		public bool CancelDiscovery { get; set; }

		public DiscoveryContext(IPEndPoint sender, DeviceStateServiceResponse response, int expectedCount)
		{
			this.Sender = sender;
			this.Response = response;
			this.ExpectedCount = expectedCount;
		}
	}//class


}//ns
