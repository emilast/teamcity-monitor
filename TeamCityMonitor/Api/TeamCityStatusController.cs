using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TeamCityMonitor.Models;
using TeamCityMonitor.ServiceReferences.TeamCity;
using TeamCityMonitor.Services;

namespace TeamCityMonitor.Api
{

	public class TeamCityStatusController : ApiController
	{
		private readonly TeamCityStatus _teamCityStatusService;

		public TeamCityStatusController()
		{
			_teamCityStatusService = new TeamCityStatus();
		}


		// GET: api/TeamCityStatus/builds
		[Route("api/TeamCityStatus/builds")]
		public Task<IEnumerable<Project>> GetBuilds()
		{
			return _teamCityStatusService.GetTeamCityStatus();
		}

		// GET: api/Status
		[Route("api/TeamCityStatus/projects")]
		public async Task<IEnumerable<Project>> GetProjects()
		{
			var cctrayProjects = await _teamCityStatusService.GetTeamCityProjects();

			var projects = MapToProjects(cctrayProjects);
			return projects;

		}

		private IEnumerable<Project> MapToProjects(IEnumerable<CCTrayProject> ccTrayProjects)
		{
			Project tmpProject = null;
			foreach (var ccTrayProject in ccTrayProjects)
			{
				var names = GetProjectNameParts(ccTrayProject.name);
				var projectName = names.Item1;
				var buildName = names.Item2;

				// If the new ccTray project has a different name, then the previous project is done and can be returned
				if (tmpProject == null || tmpProject.Name != projectName)
				{
					if (tmpProject != null)
					{
						yield return tmpProject;
					}
					
					tmpProject = new Project {Name = projectName};
				}


				var buildConfig = new BuildConfig
				{
					Name = buildName,
					Finished = ccTrayProject.lastBuildTime,
					BuildNumber = ccTrayProject.lastBuildLabel,
					Status = MapToBuildConfigStatus(ccTrayProject),
					FinishedRecently = GetRecentlyFinished(ccTrayProject.lastBuildTime)
				};
				tmpProject.BuildConfigs.Add(buildConfig);
			}

			// Return the last project
			if (tmpProject != null)
			{
				yield return tmpProject;
			}
		}

		private static string GetRecentlyFinished(string lastBuildTime)
		{
			if (string.IsNullOrEmpty(lastBuildTime))
				return null;

			//DateTimeOffset parsedDate = DateTimeOffset.ParseExact(
			//	lastBuildTime,
			//	"yyyyMMdd'T'HHmmsszzz",
			//	CultureInfo.InvariantCulture,
			//	DateTimeStyles.RoundtripKind);
			var parsedDate = DateTime.Parse(lastBuildTime);
			var timeSinceFinished = DateTime.Now - parsedDate;

			if (timeSinceFinished < TimeSpan.FromMinutes(10)) { return "now"; }
			if (timeSinceFinished < TimeSpan.FromMinutes(60)) { return "recent"; }

			if (parsedDate.Date == DateTime.Today) { return "today"; }

			return string.Empty;
		}

		private string MapToBuildConfigStatus(CCTrayProject ccTrayProject)
		{
			if (ccTrayProject.activity == "Building")
				return "Running";

			switch (ccTrayProject.lastBuildStatus)
			{
				case "Success":
					return "Success";
				case "Failure":
					return "Failure";
				default:
					return "Unknown";
			}
		}

		/// <summary>
		/// Retrieve parts of a merged name such as "Xena :: Services :: BookingService :: 2 Deploy to CI".
		/// 
		/// </summary>
		/// <param name="mergedName">Exanple: "Xena :: Services :: BookingService :: 2 Deploy to CI"</param>
		/// <returns></returns>
		private Tuple<string,string> GetProjectNameParts(string mergedName)
		{
			var parts = mergedName.Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries)
				.Select(part => part.Trim())
				.ToArray();

			var projectName = string.Join(" / ", parts.Take(parts.Count() - 1));
			var buildName = parts.Last();

			return new Tuple<string, string>(projectName, buildName);
		}
	}
}