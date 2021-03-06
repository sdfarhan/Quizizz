namespace Quizizz.Web
{
    using System;
    using System.Reflection;

    using Hangfire;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Quizizz.Common;
    using Quizizz.Data;
    using Quizizz.Data.Common;
    using Quizizz.Data.Common.Repositories;
    using Quizizz.Data.Models;
    using Quizizz.Data.Repositories;
    using Quizizz.Data.Seeding;
    using Quizizz.Services.Answers;
    using Quizizz.Services.Categories;
    using Quizizz.Services.Data;
    using Quizizz.Services.Events;
    using Quizizz.Services.EventsGroups;
    using Quizizz.Services.Groups;
    using Quizizz.Services.Mapping;
    using Quizizz.Services.Messaging;
    using Quizizz.Services.Questions;
    using Quizizz.Services.Quizzes;
    using Quizizz.Services.ScheduledJobsService;
    using Quizizz.Services.StudentsGroups;
    using Quizizz.Services.Tools.Expressions;
    using Quizizz.Services.Users;
    using Quizizz.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(
                options => options.UseSqlServerStorage(this.configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    SchemaName = "hangfire",
                }));

            services.AddHangfireServer();

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton(this.configuration);

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromDays(GlobalConstants.CookieTimeOut);
                options.Cookie.IsEssential = true;
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddDistributedMemoryCache();


            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(new LoggerFactory(), this.configuration["Sendgrid"]));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IExpressionBuilder, ExpressionBuilder>();
            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IEventsGroupsService, EventsGroupsService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IQuizzesService, QuizzesService>();
            services.AddTransient<IScheduledJobsService, ScheduledJobsService>();
            services.AddTransient<IStudentsGroupsService, StudentsGroupsService>();
            services.AddTransient<IUsersService, UsersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
