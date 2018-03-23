using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HttpTest.Startup))]
namespace HttpTest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
