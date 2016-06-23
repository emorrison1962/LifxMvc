using LifxMvc.Domain;
using LifxMvc.Services;
using LifxMvc.Services.Discovery;
using LifxMvc.Services.UdpHelper;
using LifxNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LifxMvc.Services
{
	public class DiscoveryService : IDisposable, IDiscoveryService
	{
		List<IBulb> Bulbs { get; set; }
		ManualResetEventSlim Wait { get; set; }



		public List<IBulb> DiscoverAsync(int expectedCount)
		{
			this.Bulbs = new List<IBulb>();
			var packet = new DeviceGetServicePacket();

			var udp = UdpHelperManager.Instance.DiscoveryUdpHelper;
			udp.DeviceDiscovered += Udp_DeviceDiscovered;

			this.Wait = new ManualResetEventSlim(false);
			this._sw = Stopwatch.StartNew();
			udp.DiscoverBulbs(packet, expectedCount, 10 * 1000);
			var result = this.Bulbs;

			return result;
		}

		Stopwatch _sw;
		private void Udp_DeviceDiscovered(object sender, DiscoveryEventArgs e)
		{
			var ctx = e.DiscoveryContext;
			if (null == this.Bulbs.FirstOrDefault(x => x.IPEndPoint.ToString() == ctx.Sender.ToString()))
			{
				var bulb = new Bulb()
				{
					IPEndPoint = ctx.Sender,
					Service = ctx.Response.Service,
					Port = ctx.Response.Port,
					TargetMacAddress = ctx.Response.TargetMacAddress,
					LastSeen = DateTime.UtcNow
				};

				var bulbSvc = new BulbService();
				bulbSvc.Initialize(bulb);

				this.Bulbs.Add(bulb);
				Debug.WriteLine(Bulbs.Count);
				if (this.Bulbs.Count == ctx.ExpectedCount)
				{
					_sw.Stop();
					Debug.WriteLine(_sw.ElapsedMilliseconds);


					var udp = sender as DiscoveryUdpHelper;
					udp.DeviceDiscovered -= this.Udp_DeviceDiscovered;
					ctx.CancelDiscovery = true;
					this.Wait.Set();
				}
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~DiscoveryService() {
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
}//ns
