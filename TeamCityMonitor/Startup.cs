using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeamCityMonitor.Startup))]
namespace TeamCityMonitor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			//ConfigureAuth(app);
        }
    }
}
