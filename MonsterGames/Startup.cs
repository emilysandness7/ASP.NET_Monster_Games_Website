using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MonsterGames.Startup))]
namespace MonsterGames
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
