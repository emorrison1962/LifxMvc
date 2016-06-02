using LifxMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LifxMvc.Models
{
	public class BulbsViewModel
	{
		public List<GroupViewModel> Groups { get; private set; }

		public BulbsViewModel(List<Bulb> bulbs)
		{
			this.Groups = new List<GroupViewModel>();
			var groupNames = (
				from bulb in bulbs
				select bulb.Group).Distinct();

			foreach (var groupName in groupNames)
			{
				var groupBulbs = bulbs.Where(x => x.Group == groupName).ToList();
				this.Groups.Add(new GroupViewModel(groupName, groupBulbs));
			}

		}
	}

	public class GroupViewModel
	{
		public string Name { get; private set; }
		public List<Bulb> Bulbs { get; private set; }

		public GroupViewModel(string name, List<Bulb> bulbs)
		{
			this.Bulbs = new List<Bulb>();
			this.Name = name;
			this.Bulbs = bulbs;
			this.Bulbs.OrderBy(x => x.Label);
		}
	}


}