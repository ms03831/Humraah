using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Humraah.Startup))]
namespace Humraah
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
