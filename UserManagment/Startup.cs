using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Manager.Entities;
using Manager.Services;
using FluentValidation;
using Manager.Models;
using Manager.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Vereyon.Web;
using Manager.Middleware;
using FluentEmail.MailKitSmtp;
using Manager.Models.CustomValidators;
using System;

namespace Manager
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.Cookie.Name = "Manager";
                       options.LoginPath = "/Account/Login";
                       options.LogoutPath = "/";
                       options.SlidingExpiration = true;
                       options.AccessDeniedPath = "/Account/Login";
                   });
            services.AddControllersWithViews().AddFluentValidation().AddRazorRuntimeCompilation(); 
            services.AddDbContext<ManagerDbContext>(options =>
                {
                    bool isAzure = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ConnectionStrings") is not null;
                    if(isAzure)
                        options.UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ConnectionStrings"));
                    else
                        options.UseSqlServer(Configuration.GetConnectionString("ManagerDbConnection"));
                });
            services.AddScoped<DBSeeder>();
           
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISerService, SerService>();
            services.AddScoped<IFileService, FileService>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordValidator>();
            services.AddScoped<IValidator<CategoryDto>, CreateCategory>();
            services.AddScoped<IValidator<ServiceDto>, CreateService>();
            services.AddScoped<ErrorLoggingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddFlashMessage();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserContextService, UserContextService>();

            var mailSettings = new MailSettings();
            Configuration.GetSection("MailSettings").Bind(mailSettings);
            services.AddSingleton(mailSettings);

            services.AddFluentEmail(mailSettings.From, mailSettings.DisplayName)
                .AddRazorRenderer()
                .AddMailKitSender(new SmtpClientOptions
                {
                    Server = mailSettings.Host,
                    Port = mailSettings.Port,
                    UseSsl = mailSettings.UseSsl,
                    RequiresAuthentication = mailSettings.RequiresAuthentication,
                    User = mailSettings.From,
                    Password = mailSettings.Password
                });
            services.AddScoped<IMailSenderService, MailSenderService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DBSeeder seeder)
        {

            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/ExeptionPage");
                app.UseHsts();
            }
            
            app.UseMiddleware<ErrorLoggingMiddleware>();
            app.UseStatusCodePagesWithRedirects("/Error/Code/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

           

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                Secure = CookieSecurePolicy.Always,
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "manager",
                    pattern: "manager/{controller=Manager}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");


            });
        }
    }
}
