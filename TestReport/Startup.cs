using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestReport.Startup))]
namespace TestReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
