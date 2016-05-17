using LifxMvc.Domain;
using LifxMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifxMvc.Controllers
{
	public class BulbController : Controller
	{

		List<Bulb> GetBulbs()
		{
			var svc = new DiscoveryService();
			List<Bulb> bulbs = svc.DiscoverAsync(8);

			var bulbSvc = new BulbService();
			bulbs.ForEach(x => bulbSvc.LightGet(x));
			return bulbs;
		}

		// GET: Bulb
		public ActionResult Index()
		{
			var bulbs = GetBulbs();
			return View(bulbs);
		}

		public ActionResult SetPower()
		{
			var bulbs = GetBulbs();
			var svc = new BulbService();
			var bulb = bulbs[0];
			svc.LightSetPower(bulb, !bulb.IsOn);

			return View(bulbs);
		}


		[HttpPost]
		public ActionResult Index(List<Bulb> list)
		{
			return View(list);
		}

	}
}