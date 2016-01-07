using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamCityMonitor.ServiceReferences.TeamCity
{
	public class TeamCityService
	{

		private static string ApiBaseAddress
		{
			get { return "/httpAuth/app/rest"; }
		}

		private readonly HttpClient _http;

		public TeamCityService()
		{
			var teamCityHost = ConfigurationManager.AppSettings["TeamCityHost"];
			var teamCityUser = ConfigurationManager.AppSettings["TeamCityUser"];
			var teamCityPassword = ConfigurationManager.AppSettings["TeamCityPassword"];

			_http = new HttpClient
			{
				BaseAddress = new Uri(teamCityHost)
			};
			_http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(
					System.Text.Encoding.ASCII.GetBytes(
						string.Format("{0}:{1}", teamCityUser, teamCityPassword))));
		}

		public async Task<IEnumerable<BuildType>> GetBuildTypes()
		{
			var response = await _http.GetAsync(ApiBaseAddress + "/buildTypes");
			var content = await response.Content.ReadAsStringAsync();

			var buildTypesResponse = JsonConvert.DeserializeObject<BuildTypesResponse>(content);

			return buildTypesResponse.buildType;
		}

		public async Task<DetailedBuild> GetLastBuildForBuildType(string buildTypeId)
		{
			var response =
				await _http.GetAsync(ApiBaseAddress + "/buildTypes/" + buildTypeId + "/builds?locator=count:1,running:any");
			var content = await response.Content.ReadAsStringAsync();

			// Get last build (only most important fields are returned)
			var buildsResponse = JsonConvert.DeserializeObject<BuildsResponse>(content);
			var build = buildsResponse.build.FirstOrDefault();

			if (build == null)
				return null;

			// Now that we know the build's id, we can get the detailed information such as finishing time, etc
			// We only get this information when requesting a single build using it's id.
			var detailedBuildResponse = await _http.GetAsync(ApiBaseAddress + "/builds/" + build.id);
			var detailedBuildContent = await detailedBuildResponse.Content.ReadAsStringAsync();
			var detailedBuild = JsonConvert.DeserializeObject<DetailedBuild>(detailedBuildContent);

			return detailedBuild;
		}

		public async Task<IEnumerable<CCTrayProject>> GetCCTrayProjects()
		{
			var response = await _http.GetAsync(ApiBaseAddress + "/cctray/projects.xml");
			var content = await response.Content.ReadAsStringAsync();

			var responseObject = JsonConvert.DeserializeObject<CCTrayProjectResponse>(content);

			return responseObject.Project;
		}
	}
}

