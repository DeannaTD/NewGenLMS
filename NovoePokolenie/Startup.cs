using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using NovoePokolenie.Services;
using NovoePokolenie.UoW;

namespace NovoePokolenie
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
            services.AddDbContext<NewGenContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<NPUser, IdentityRole>(
               opts =>
               {
                   opts.Password.RequireDigit = false;
                   opts.Password.RequiredLength = 3;
                   opts.Password.RequireNonAlphanumeric = false;
                   opts.Password.RequireUppercase = false;
                   
               }).AddEntityFrameworkStores<NewGenContext>()
                 .AddTokenProvider<DataProtectorTokenProvider<NPUser>>(TokenOptions.DefaultProvider);

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(3600);
                options.Cookie.HttpOnly= true;
                options.Cookie.IsEssential = true;
            });


            services.AddTransient<UnitOfWork>();
            services.AddTransient<LeadService>();
            services.AddTransient<AccountService>();
            services.AddTransient<BranchService>();
            services.AddTransient<GroupService>();
            services.AddTransient<TimeTableService>();
            services.AddTransient<StudentService>();
            services.AddTransient<AttendanceService>();
            services.AddTransient<PaymentService>();
            services.AddTransient<PaymentPeriodService>();
            services.AddTransient<ProjectService>();
            services.AddTransient<LevelService>();
            services.AddTransient<UserProjectService>();
            services.AddTransient<CommentService>();
            services.AddTransient<StatusHistoryService>();
            services.AddControllersWithViews();
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
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }

        
    }
}