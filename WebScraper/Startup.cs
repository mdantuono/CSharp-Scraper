using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebScraper.Startup))]
namespace WebScraper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
