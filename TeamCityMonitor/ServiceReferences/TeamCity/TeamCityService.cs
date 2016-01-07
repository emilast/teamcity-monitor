using System;
using System.Collections.Generic;
using System.Configuration;
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

		public async Task<IEnumerable<CCTrayProject>> GetCCTrayProjects()
		{
			var response = await _http.GetAsync(ApiBaseAddress + "/cctray/projects.xml");
			var content = await response.Content.ReadAsStringAsync();

			var responseObject = JsonConvert.DeserializeObject<CCTrayProjectResponse>(content);

			return responseObject.Project;
		}
	}
}

