using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tephraSystemEditor.Areas.Identity.Data;
using Pomelo.EntityFrameworkCore;

[assembly: HostingStartup(typeof(tephraSystemEditor.Areas.Identity.IdentityHostingStartup))]
namespace tephraSystemEditor.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<tephraSystemEditorIdentityDbContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("default")));
            });

        }
    }
}