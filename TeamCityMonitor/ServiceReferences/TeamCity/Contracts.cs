using System.Collections.Generic;

namespace TeamCityMonitor.ServiceReferences.TeamCity
{
	public class BuildTypesResponse
	{
		public int count { get; set; }
		public IEnumerable<BuildType> buildType { get; set; }
	}

	public class BuildType
	{
		public string id { get; set; }
		public string name { get; set; }
		public string projectId { get; set; }
		public string projectName { get; set; }
		public string href { get; set; }
		public string webUrl { get; set; }
	}


	public class BuildsResponse
	{
		public int count { get; set; }
		public IEnumerable<Build> build { get; set; }
	}


	public class Build
	{
		public string id { get; set; }
		public string buildTypeId { get; set; }
		public string number { get; set; }

		/// <summary>
		///     Status: "SUCCESS", "FAILURE"
		/// </summary>
		public string status { get; set; }

		/// <summary>
		///     State: "running", "finished"
		/// </summary>
		public string state { get; set; }

		public string href { get; set; }
		public string webUrl { get; set; }
	}

	public class DetailedBuild : Build
	{
		public string startDate { get; set; }
		public string finishDate { get; set; }

		public BuildTriggered triggered { get; set; }
	}

	public class BuildTriggered
	{
		public string type { get; set; }
		public string date { get; set; }
		public User user { get; set; }
	}

	public class User
	{
		public string username { get; set; }
		public string name { get; set; }
	}




	public class CCTrayProjectResponse
	{
		public IEnumerable<CCTrayProject> Project { get; set; }
	}

	public class CCTrayProject
	{
		public string name { get; set; }
		public string lastBuildStatus { get; set; }
		public string lastBuildLabel { get; set; }
		public string lastBuildTime { get; set; }
		public string webUrl { get; set; }

		/// <summary>
		/// Sleeping, Building, Failed?
		/// </summary>
		public string activity { get; set; }
	}
}