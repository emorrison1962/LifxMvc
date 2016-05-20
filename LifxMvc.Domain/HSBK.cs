using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxMvc.Domain
{
	public class HSBK
	{
		public UInt16 Hue { get; private set; }
		public UInt16 Saturation { get; private set; }
		public UInt16 Brightness { get; private set; }
		public UInt16 Kelvin { get; private set; }

		public HSBK(UInt16 h, UInt16 s, UInt16 b, UInt16 k)
		{
			this.Hue = h;
			this.Saturation = s;
			this.Brightness = b;
			this.Kelvin = k;
		}
		public void RotateHue()
		{
			if (0 != this.Saturation)
			{
				this.Hue += (UInt16)64;// Int16.MaxValue;
									   //Console.Beep();
			}
		}

		public void RotateHue(UInt16 degrees)
		{
			UInt16 degreeWidth = UInt16.MaxValue / 360;
			UInt16 rotation = (UInt16)(degreeWidth * degrees);

			if (0 != this.Saturation)
			{
				this.Hue += rotation;
									   //Console.Beep();
			}
		}



	}
}
