using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebQuanLyThuVien.Startup))]
namespace WebQuanLyThuVien
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
