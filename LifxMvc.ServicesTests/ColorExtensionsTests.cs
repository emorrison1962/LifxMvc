using Microsoft.VisualStudio.TestTools.UnitTesting;
using LifxMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Eric.Morrison;
using System.Diagnostics;
using Colorspace;
using LifxMvc.Domain;

namespace LifxMvc.Services.Tests
{
	[TestClass()]
	public class ColorExtensionsTests
	{
		const int MAX_COUNT = 100 * 10000;

		[TestMethod()]
		public void ToHSBKTest()
		{
			for (int i = 0; i < MAX_COUNT; ++i)
			{
				//var a = Color.FromArgb(RandomValue.Next(0, 256), RandomValue.Next(0, 256), RandomValue.Next(0, 256));
				//var rgb = new ColorRGB(a.ToArgb());
				//var hsl = new ColorHSL(rgb);
				//var rgb2 = new ColorRGB(hsl);
				//var rgb32 = new ColorRGB32Bit(rgb2);
				//var b = Color.FromArgb(rgb32.ToInt());



				var a = Color.FromArgb(RandomValue.Next(0, 256), RandomValue.Next(0, 256), RandomValue.Next(0, 256));
				var hsbk = a.ToHSBK();
				var b = hsbk.ToColor();
				if ((a.R != b.R) || (a.B != b.B) || (a.G != b.G))
				{
					Debug.WriteLine(i.ToString());
					Debug.WriteLine(a.ToString());
					Debug.WriteLine(b.ToString());

					new object();
				}

				Assert.AreEqual(a.R, b.R);
				Assert.AreEqual(a.G, b.G);
				Assert.AreEqual(a.B, b.B);

				new object();
			}
		}

		[TestMethod()]
		public void ToColorHSLTest()
		{
			Assert.Fail();
		}

		[TestMethod()]
		public void ToColorTest()
		{
			Assert.Fail();
		}
	}
}