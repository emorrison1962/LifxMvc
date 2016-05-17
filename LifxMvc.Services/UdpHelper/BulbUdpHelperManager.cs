using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LifxMvc.Services.UdpHelper
{
	public class UdpHelperManager : IDisposable
	{
		static public UdpHelperManager Instance { get; private set; }
		Dictionary<IPEndPoint, BulbUdpHelper> Dictionary { get; set; }

		DiscoveryUdpHelper _discoveryUdpHelper;
		public DiscoveryUdpHelper DiscoveryUdpHelper
		{
			get
			{
				if (null == _discoveryUdpHelper)
					_discoveryUdpHelper = new DiscoveryUdpHelper();
				return _discoveryUdpHelper;
			}
		}

		static UdpHelperManager()
		{
			Instance = new UdpHelperManager();
		}

		UdpHelperManager()
		{
			Dictionary = new Dictionary<IPEndPoint, BulbUdpHelper>();
		}

		public BulbUdpHelper this[IPEndPoint key]
		{
			get { return this.Lookup(key); }
		}

		BulbUdpHelper Lookup(IPEndPoint key)
		{
			BulbUdpHelper result = null;
			if (this.Dictionary.ContainsKey(key))
			{
				result = this.Dictionary[key];
			}
			else
			{
				result = new BulbUdpHelper(key);
				this.Dictionary.Add(key, result);
			}
			return result;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					var list = this.Dictionary.Values.ToList();
					list.ForEach(x => x.Dispose());
					if (null != _discoveryUdpHelper)
						_discoveryUdpHelper.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~BulbUdpHelperManager() {
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
