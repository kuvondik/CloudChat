using Dna;
using Dna.AspNet;
using CloudChat.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using static Dna.FrameworkDI;

namespace CloudChat.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSendGridEmailSender();
            services.AddEmailTemplateSender();

            //AddItentity adds cookie based authentication
            //Adds scoped classes for things like UserManager, SignInManager, PasswordHashers, etc...
            //NOTE: Automatically adds the validated user from a cookie to the HttpContext.User
            services.AddIdentity<ApplicationUser, IdentityRole>()
                //Adds UserStore and RoleStore from this context
                //That are consumed by the UserManager and  RoleManager
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // Adds a provider that generates unique keys and hashes for things for like 
                //forgot password links, phone number verification codes etc...
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                    .AddJwtBearer(option =>
                            option.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = Framework.Construction.Configuration["Jwt:Issuer"],
                                ValidAudience = Framework.Construction.Configuration["Jwt:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(Framework.Construction.Configuration["Jwt:SecretKey"])),
                            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationDbContext>(option =>
               option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ChatMessageDbContext>(option =>
               option.UseSqlServer(Configuration.GetConnectionString("ChatDatabaseConnection")));

            services.AddTransient<IDataRepository, EFMessageRepository>();
            services.AddSingleton<List<MessageApiModel>>();
            services.AddSingleton<List<ConversationApiModel>>();
            services.AddSingleton<Dictionary<string, IEnumerable<string>>>();
            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequiredLength = 1;
                option.User.RequireUniqueEmail = false;
            });
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/login";
                option.ExpireTimeSpan = TimeSpan.FromSeconds(5);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseDnaFramework();

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
            app.UseCookiePolicy();

            //Setup Identity
            app.UseAuthentication();

            //Setup Session

            //Setup MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Make sure we have the database
            serviceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();
            serviceProvider.GetService<ChatMessageDbContext>().Database.EnsureCreated();
        }
    }
}
