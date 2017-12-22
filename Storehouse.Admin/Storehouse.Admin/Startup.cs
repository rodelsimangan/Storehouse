using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StorehouseAdmin.Startup))]
namespace StorehouseAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
