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

		List<Bulb> GetBulbs()
		{
			List<Bulb> bulbs = new List<Bulb>();
			using (var svc = new DiscoveryService())
			{
				bulbs = svc.DiscoverAsync(8);

				var bulbSvc = new BulbService();
				bulbs.ForEach(x => bulbSvc.LightGet(x));
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

		//[HttpPost]
		public ActionResult TogglePower(int bulbId)
		{
			var bulb = this.Bulbs.FirstOrDefault(x => x.BulbId == bulbId);

			var svc = new BulbService();
			svc.LightSetPower(bulb, !bulb.IsOn);
			return RedirectToAction("Index");
		}



		[HttpPost]
		public ActionResult Index(List<Bulb> list)
		{
			return View(list);
		}

	}
}