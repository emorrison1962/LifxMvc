using LifxMvc.Domain;
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
	public class DiscoveryService
	{
		List<Bulb> Bulbs { get; set; }
		ManualResetEventSlim Wait { get; set; }

		public List<Bulb> DiscoverAsync(int expectedCount)
		{
			this.Bulbs = new List<Bulb>();
			var packet = new DeviceGetServicePacket();

			var udp = UdpHelperManager.Instance.DiscoveryUdpHelper;
			udp.DeviceDiscovered += Udp_DeviceDiscovered;

			this.Wait = new ManualResetEventSlim(false);
			var sw = Stopwatch.StartNew();
			udp.BroadcastAndListen(packet, this.DiscoveryCallback, expectedCount, 10 * 1000);
			sw.Stop();
			Debug.WriteLine(sw.ElapsedMilliseconds);
			var result = this.Bulbs;
			return result;
		}

		private void Udp_DeviceDiscovered(object sender, DiscoveryUdpHelper.DiscoveryEventArgs e)
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

				this.Bulbs.Add(bulb);
				Debug.WriteLine(Bulbs.Count);
				if (this.Bulbs.Count == ctx.ExpectedCount)
				{
					var udp = sender as DiscoveryUdpHelper;
					udp.DeviceDiscovered -= this.Udp_DeviceDiscovered;
					ctx.CancelDiscovery = true;
					this.Wait.Set();
				}
			}
		}

		void DiscoveryCallback(DiscoveryContext ctx)
		{
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

				this.Bulbs.Add(bulb);
				Debug.WriteLine(Bulbs.Count);
				if (this.Bulbs.Count == ctx.ExpectedCount)
				{
					ctx.CancelDiscovery = true;
					this.Wait.Set();
				}
			}
		}

	}//class
}//ns
