using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Infrastructure.DAL;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace MarinIDP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new DefaultContractResolver(); //Makes all json objects look exactly the same as the original .net object. Keeps CamelCase on properties for example.
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(Configuration);

            services.AddIdentity<MarinAppUser, IdentityRole>().AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:IdentityConnectionString"]));
            services.Configure<IdentityOptions>(options =>
            {

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie().AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (Configuration["JwtKey"]))
                };
            });

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IAuthManager, AuthManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(builder =>
            {
                builder.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}");
            });

            try
            {
                //Run latest migration 
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var securityDbContext = serviceScope.ServiceProvider.GetRequiredService<SecurityDbContext>();
                    securityDbContext.Database.Migrate();

                    if (!securityDbContext.Users.Any(x => x.Email.Equals("janneb@mailinator.com")))
                    {
                        var um = serviceScope.ServiceProvider.GetRequiredService<UserManager<MarinAppUser>>();
                        var appUser = new MarinAppUser("janneb@mailinator.com", "Jan", "Banan") { EmailConfirmed = true };

                        um.CreateAsync(appUser, "Test1234").Wait();
                        um.AddClaimAsync(appUser, new Claim("UserId", appUser.Id)).Wait();
                        um.AddClaimAsync(appUser, new Claim("Name", appUser.GetFullName())).Wait();
                        um.UpdateAsync(appUser).Wait();

                        securityDbContext.SaveChanges();


                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
