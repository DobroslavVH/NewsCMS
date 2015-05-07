using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsCMS.Startup))]
namespace NewsCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
