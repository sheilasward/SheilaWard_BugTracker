using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SheilaWard_BugTracker.Startup))]
namespace SheilaWard_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
