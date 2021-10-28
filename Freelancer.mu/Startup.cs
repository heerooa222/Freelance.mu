using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freelancer.mu.Classes.Notification;
using Freelancer.mu.Classes.Notification.Connection;
using Freelancer.mu.Classes.Notification.Sender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Freelancer.DBProvider.Competence;
using Services.Freelancer.DBProvider.Message;
using Services.Freelancer.DBProvider.Project;
using Services.Freelancer.DBProvider.Type;
using Services.Freelancer.DBProvider.User;
using Services.Freelancer.Entities;
using Services.Freelancer.Services.Message;
using Services.Freelancer.Services.Project;
using Services.Freelancer.Services.Type;
using Services.Freelancer.Services.User;

namespace Freelancer.mu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(60));
            //Services
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITypeService, TypeService>();
            services.AddTransient<IMessageService, MessageService>();
            
            //Data provider
            services.AddTransient<ICompetenceDataProvider, CompetenceDataProvider>();
            services.AddTransient<IProjectDataProvider, ProjectDataProvider>();
            services.AddTransient<IUserDataProvider, UserDataProvider>();
            services.AddTransient<ITypeDataProvider, TypeDataProvider>();
            services.AddTransient<IMessageDataProvider, MessageDataProvider>();
                
            services.AddDbContext<DatabaseContext>(options => options.UseMySQL("server=localhost;port=3307;user=root;password=root;database=freelancer"));
            
            //Notification
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<IUserConnectionManager, UserConnectionManager>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                await next();
                switch (context.Response.StatusCode)
                {
                    case 404:
                        context.Response.Redirect( $"/error/code404?req={context.Request.Path}");
                        break;
                    case 500:
                        context.Response.Redirect($"/error/code500?req={context.Request.Path}");
                        break;
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}