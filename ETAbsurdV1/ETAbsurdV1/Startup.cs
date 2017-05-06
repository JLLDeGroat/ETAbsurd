using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ETAbsurdV1.Startup))]
namespace ETAbsurdV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
