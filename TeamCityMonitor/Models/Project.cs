using System.Collections.Generic;

namespace TeamCityMonitor.Models
{
	public class Project
	{
		public string Name { get; set; }
		//public BuildStatus Status { get; set; }
		public IList<BuildConfig> BuildConfigs { get; set; }

		public Project()
		{
			BuildConfigs = new List<BuildConfig>();
		}
	}
}