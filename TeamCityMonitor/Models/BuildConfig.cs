using System;

namespace TeamCityMonitor.Models
{
	public class BuildConfig
	{
		public string Name { get; set; }
		public string Status { get; set; }
		public string BuildId { get; set; }
		public string BuildNumber { get; set; }
		public string Finished { get; set; }
		public string Trigger { get; set; }

		public string FinishedRecently { get; set; }
	}
}