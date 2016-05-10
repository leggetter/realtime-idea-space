using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(realtime_idea_space.Startup))]
namespace realtime_idea_space
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
