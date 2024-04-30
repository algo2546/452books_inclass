using BooksSpring2024_sec02.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

namespace BooksSpring2024_sec02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //add services to the container
            builder.Services.AddControllersWithViews();

            //fetch the information
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");

            //add the context class to the set of services and define the option to use SQL Server
            builder.Services.AddDbContext<BooksDBContext>(options => options.UseSqlServer(connString));


            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));



            builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<BooksDBContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });



            builder.Services.AddRazorPages();



            builder.Services.AddScoped<IEmailSender, EmailSender>(); //this is to avoid an error

            var app = builder.Build();

            //configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //the default HSTS value is 30 days
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();


            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();






            app.MapControllerRoute(
                name: "default",
                pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
