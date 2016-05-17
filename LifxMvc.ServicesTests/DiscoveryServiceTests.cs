using Microsoft.VisualStudio.TestTools.UnitTesting;
using LifxMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifxMvc.Domain;
using System.Threading;
using LifxMvc.Services.UdpHelper;

namespace LifxMvc.Services.Tests
{
	[TestClass()]
	public class DiscoveryServiceTests
	{
		#region Fields

		const int EXPECTED_BULB_COUNT = 8;
		BulbService _svc;

		#endregion


		#region Properties

		static List<Bulb> Bulbs { get; set; }

		BulbService BulbService
		{
			get
			{
				if (null == _svc)
					_svc = new BulbService();
				return _svc;
			}
		}

		#endregion


		#region Setup/ teardown

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			GetBulbs();
		}

		static void GetBulbs()
		{
			var svc = new DiscoveryService();

			var result = svc.DiscoverAsync(EXPECTED_BULB_COUNT);
			Bulbs = new List<Bulb>(result);
			Bulbs.Sort(new BulbComparer());
		}

		[ClassCleanup()]
		public static void ClassCleanup()
		{
			UdpHelperManager.Instance.Dispose();
		}

		#endregion

		#region Utils

		class BulbComparer : IComparer<Bulb>
		{
			public int Compare(Bulb x, Bulb y)
			{
				return x.IPEndPoint.ToString().CompareTo(y.IPEndPoint.ToString());
			}
		}

		void LightSetPower(Bulb bulb, bool power)
		{
			BulbService.LightSetPower(bulb, power);
		}

		bool LightGetPower(Bulb bulb)
		{
			var result = BulbService.LightGetPower(bulb);
			return result;
		}

		void DeviceSetPower(Bulb bulb, bool power)
		{
			BulbService.DeviceSetPower(bulb, power);
		}

		bool DeviceGetPower(Bulb bulb)
		{
			var result = BulbService.DeviceGetPower(bulb);
			return result;
		}

		void TurnOn(Bulb bulb)
		{
			if (!bulb.IsOn)
				this.DeviceSetPower(bulb, true);
		}

		void TurnOff(Bulb bulb)
		{
			if (!bulb.IsOn)
				this.DeviceSetPower(bulb, false);
		}


		#endregion

		[TestMethod()]
		public void DiscoveryTest()
		{
			//GetBulbs();
			Assert.AreEqual(EXPECTED_BULB_COUNT, Bulbs.Count);
		}

		[TestMethod()]
		public void LightGetTest()
		{
			var expectedCount = Bulbs.Count;
			var count = Bulbs.Where(x => null == x.Label).Count();
			Assert.AreEqual(expectedCount, count);

			foreach (var bulb in Bulbs)
			{
				BulbService.LightGet(bulb);
			}

			count = Bulbs.Where(x => null != x.Label).Count();
			Assert.AreEqual(expectedCount, count);
		}


		[Ignore]
		[TestMethod()]
		public void LightSetColorTest()
		{
			Bulbs.ForEach(x => this.TurnOn(x));
			for (int x = 0; x < 10; ++x)
			{
				for (int i = 0; i < UInt16.MaxValue / 64; ++i)
				{
					foreach (var bulb in Bulbs)
					{
						if (bulb.Label == "1 o'clock")
							new object();

						var hsbk = new HSBK(bulb.Hue, bulb.Saturation, bulb.Brightness, bulb.Kelvin);
						hsbk.RotateHue();
						BulbService.LightSetColor(bulb, hsbk);
					}
				}
			}

		}


		[TestMethod()]
		public void DeviceGetVersionTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.DeviceGetVersion(bulb);

				Assert.IsNotNull(bulb.Vendor);
				Assert.IsNotNull(bulb.Product);
				Assert.IsNotNull(bulb.Version);
			}
		}

		[TestMethod()]
		public void GetHostInfoTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetHostInfo(bulb);

				Assert.AreNotEqual(Single.MaxValue, bulb.Signal);
				Assert.AreNotEqual(UInt32.MaxValue, bulb.TxCount);
				Assert.AreNotEqual(UInt32.MaxValue, bulb.RxCount);
			}
		}

		[TestMethod()]
		public void GetHostFirmwareTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetHostFirmware(bulb);

				Assert.AreNotEqual(DateTime.MaxValue, bulb.HostFirmwareBuild);
				Assert.AreNotEqual(UInt32.MaxValue, bulb.HostFirmwareVersion);
			}
		}

		[TestMethod()]
		public void GetWifiInfoTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetWifiInfo(bulb);

				Assert.AreNotEqual(Single.MaxValue, bulb.WifiInfoSignal);
				Assert.AreNotEqual(UInt32.MaxValue, bulb.WifiInfoTxCount);
				Assert.AreNotEqual(UInt32.MaxValue, bulb.WifiInfoRxCount);
			}
		}

		[TestMethod()]
		public void GetWifiFirmwareTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetWifiFirmware(bulb);

				Assert.AreNotEqual(DateTime.MaxValue, bulb.WifiFirmwareBuild);
				Assert.AreNotEqual(UInt32.MaxValue, bulb.WifiFirmwareVersion);
			}
		}

		[TestMethod()]
		public void GetLabelTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetLabel(bulb);

				Assert.IsNotNull(bulb.Label);
			}
		}

		[TestMethod()]
		public void SetLabelTest()
		{
			foreach (var bulb in Bulbs)
			{
				var oldLabel = bulb.Label ?? string.Empty;
				var testLabel = "THIS IS A TEST";

				BulbService.SetLabel(bulb, testLabel);
				BulbService.GetLabel(bulb);
				Assert.AreEqual(testLabel, bulb.Label);

				BulbService.SetLabel(bulb, oldLabel);
				BulbService.GetLabel(bulb);
				Assert.AreEqual(oldLabel, bulb.Label);
			}
		}

		[TestMethod()]
		public void GetInfoTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetInfo(bulb);

				Assert.AreNotEqual(DateTime.MaxValue, bulb.Time);
				Assert.AreNotEqual(DateTime.MaxValue, bulb.Uptime);
				Assert.AreNotEqual(DateTime.MaxValue, bulb.Downtime);
			}
		}

		[TestMethod()]
		public void EchoRequestTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.EchoRequest(bulb);
			}
		}

		[TestMethod()]
		public void LightGetPowerTest()
		{
			foreach (var bulb in Bulbs)
			{
				var power = BulbService.LightGetPower(bulb);
				Assert.AreEqual(power, bulb.IsOn);
			}
		}

		[TestMethod()]
		public void DeviceGetPowerTest()
		{
			foreach (var bulb in Bulbs)
			{
				var result = BulbService.DeviceGetPower(bulb);
				Assert.AreEqual(result, bulb.IsOn);
			}
		}

		[TestMethod()]
		public void DeviceSetPowerTest()
		{
			foreach (var bulb in Bulbs)
			{
				var requested = !bulb.IsOn;
				this.DeviceSetPower(bulb, requested);
				var expected = this.DeviceGetPower(bulb);
				Assert.AreEqual(requested, expected);
				this.DeviceSetPower(bulb, !requested); // toggle back to initial state.
			}
		}

		[TestMethod()]
		public void LightSetPowerTest()
		{
			foreach (var bulb in Bulbs)
			{
				var requested = !bulb.IsOn;
				this.LightSetPower(bulb, requested);
				var expected = this.LightGetPower(bulb);
				Assert.AreEqual(requested, expected);
				this.LightSetPower(bulb, !requested); // toggle back to initial state.
			}

		}

		[TestMethod()]
		public void GetGroupTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetGroup(bulb);
			}
		}

		[TestMethod()]
		public void GetLocationTest()
		{
			foreach (var bulb in Bulbs)
			{
				BulbService.GetLocation(bulb);
			}
		}
	}//class
}//ns