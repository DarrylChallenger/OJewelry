using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OJewelry.Startup))]
namespace OJewelry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
