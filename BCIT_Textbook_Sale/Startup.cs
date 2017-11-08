using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BCIT_Textbook_Sale.Startup))]
namespace BCIT_Textbook_Sale
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
