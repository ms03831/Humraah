using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Humraaah.Startup))]
namespace Humraaah
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
