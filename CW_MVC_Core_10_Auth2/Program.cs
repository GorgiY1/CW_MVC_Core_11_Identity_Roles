using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CW_MVC_Core_10_Auth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Shop_app
{
    public class DynamicRoleRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; }

        public DynamicRoleRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }

    public class DynamicRoleHandler : AuthorizationHandler<DynamicRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        {
            if (RolesModerator.Roles.Any(role => context.User.IsInRole(role)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    );
            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<UserContext>()
            .AddRoles<IdentityRole>();


            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddRazorPages();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("admin"));
            });
            var app = builder.Build();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    await RoleInitializer.Initialize(services);
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseStaticFiles();
            app.MapRazorPages();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }
}