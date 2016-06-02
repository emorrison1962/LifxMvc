using LifxMvc.Domain;
using LifxMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace LifxMvc.Controllers
{
	public class BulbController : Controller
	{
		const string BULBS = "BulbController.Bulbs";

		ICacheService CacheService { get; set; }
		List<Bulb> Bulbs
		{
			get { return CacheService.GetOrSet(BULBS, this.GetBulbs); }
		}


		public BulbController(ICacheService cacheSvc)
		{
			this.CacheService = cacheSvc;
		}

		class BulbComparer : IComparer<Bulb>
		{
			public int Compare(Bulb x, Bulb y)
			{
				var result = (x.Group ?? "").CompareTo((y.Group ?? ""));
				if (0 == result)
				{
					result = (x.Label ?? "").CompareTo((y.Label ?? ""));
				}

				return result;
			}
		}

		List<Bulb> GetBulbs()
		{
			List<Bulb> bulbs = new List<Bulb>();
			using (var svc = new DiscoveryService())
			{
				bulbs = svc.DiscoverAsync(8);
				bulbs.Sort(new BulbComparer());
			}
			return bulbs;
		}

		// GET: Bulb
		public ActionResult Index()
		{
			var bulbs = this.Bulbs;
			return View(bulbs);
		}

		public ActionResult SetPower()
		{
			var bulbs = this.Bulbs;
			var svc = new BulbService();
			var bulb = bulbs[0];
			svc.LightSetPower(bulb, !bulb.IsOn);

			return View(bulbs);
		}

		public ActionResult TogglePower(int bulbId)
		{
			var bulb = this.Bulbs.FirstOrDefault(x => x.BulbId == bulbId);

			var svc = new BulbService();
			svc.LightSetPower(bulb, !bulb.IsOn);
			return RedirectToAction("Index");
		}

		public ActionResult TogglePowerAll()
		{
			var isOn = this.Bulbs.Where(x => x.IsOn).Count() > 0;

			var svc = new BulbService();
			foreach (var bulb in Bulbs)
			{
				svc.LightSetPower(bulb, !isOn);
			}
			return RedirectToAction("Index");
		}

		public ActionResult TogglePowerGroup(string group)
		{
			var bulbs = this.Bulbs.Where(x => x.Group == group);
			var isOn = bulbs.Where(x => x.IsOn).Count() > 0;
			
			var svc = new BulbService();
			foreach (var bulb in bulbs)
			{
				svc.LightSetPower(bulb, !isOn);
			}

			return RedirectToAction("Index");
		}

		public ActionResult Discover()
		{
			this.CacheService.Remove(BULBS);
			var unused = this.Bulbs;
			return RedirectToAction("Index");
		}



		[HttpPost]
		public ActionResult Index(List<Bulb> list)
		{
			return View(list);
		}

	}
}