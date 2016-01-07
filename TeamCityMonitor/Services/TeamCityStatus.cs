using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TeamCityMonitor.Models;
using TeamCityMonitor.ServiceReferences.TeamCity;

namespace TeamCityMonitor.Services
{
	public class TeamCityStatus
	{
		private TeamCityService _teamCity;

				public TeamCityStatus()
		{
			_teamCity = new TeamCityService();
		}

		public async Task<IEnumerable<Project>> GetTeamCityStatus()
		{
			var projectStates = new Dictionary<string, Project>();

			var buildTypes = await _teamCity.GetBuildTypes();

			var tasks = new List<Task>();
			foreach (var buildType in buildTypes)
			{

				var dictKey = buildType.projectName;
				if (!projectStates.ContainsKey(dictKey))
				{
					projectStates.Add(dictKey, new Project
					{
						Name = buildType.projectName,
						BuildConfigs = new List<BuildConfig>()
					});
				}

				var buildConfig = new BuildConfig
				{
					Name = buildType.name,
					//Status = buildStatus.ToString()
				};
				projectStates[dictKey].BuildConfigs.Add(buildConfig);

				// Asynchronously retrive the build config's status
				BuildType type = buildType;
				var getStatusTask = Task.Factory.StartNew(() =>
				{
					var getBuildTask = _teamCity.GetLastBuildForBuildType(type.id);
					getBuildTask.Wait(10000);

					var build = getBuildTask.Result;
					buildConfig.Status = GetBuildStatus(build).ToString();
					buildConfig.BuildId = build.id;
					buildConfig.BuildNumber = build.number;

					if (!string.IsNullOrEmpty(build.finishDate))
					{
						var parsedTeamCityDate = DateTime.ParseExact(build.finishDate, "yyyyMMdd'T'HHmmsszzz",
							CultureInfo.InvariantCulture);
						buildConfig.Finished = parsedTeamCityDate.ToString("yyyy-MM-dd HH:mm:ss");
					}
					//buildConfig.Trigger = build.triggered.type + "/" + (build.triggered.user != null ? build.triggered.user.username : "");
					buildConfig.Trigger = (build.triggered.user != null ? build.triggered.user.username : "");
				});
				tasks.Add(getStatusTask);
			}

			//Task.WaitAll(tasks.ToArray());
			foreach (var task in tasks)
			{
				await task;
			}

			return projectStates.Values
				.OrderBy(project => project.Name);
		}

		private BuildStatus GetBuildStatus(Build build)
		{
			if (build.state == "running")
				return BuildStatus.Running;

			if (build.status == "SUCCESS")
				return BuildStatus.Success;

			if (build.status == "FAILURE")
				return BuildStatus.Failure;

			return BuildStatus.None;
		}

		public async Task<IEnumerable<CCTrayProject>> GetTeamCityProjects()
		{
			var projects = await _teamCity.GetCCTrayProjects();
			return projects.OrderBy(project => project.name);

		}
	}
}