using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityMonitor.ServiceReferences.TeamCity;

namespace TeamCityMonitor.Services
{
	public class TeamCityStatus
	{
		private readonly TeamCityService _teamCity;

		public TeamCityStatus()
		{
			_teamCity = new TeamCityService();
		}

		public async Task<IEnumerable<CCTrayProject>> GetTeamCityProjects()
		{
			var projects = await _teamCity.GetCCTrayProjects();
			return projects.OrderBy(project => project.name);

		}
	}
}