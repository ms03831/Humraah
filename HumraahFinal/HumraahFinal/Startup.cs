using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HumraahFinal.Startup))]
namespace HumraahFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
