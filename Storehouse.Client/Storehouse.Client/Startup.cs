using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMSPortal.Startup))]
namespace CMSPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
