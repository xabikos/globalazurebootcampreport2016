using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GlobalAzureBootcampReport.Startup))]
namespace GlobalAzureBootcampReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
