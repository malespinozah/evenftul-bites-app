using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Eventful_Bite_App.Startup))]
namespace Eventful_Bite_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
