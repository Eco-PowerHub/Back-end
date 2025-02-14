
using EcoPowerHub.AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.Repositories.Services;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;


namespace EcoPowerHub
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //add ConnectionString 
           builder.Services.AddDbContext<EcoPowerDbContext>(options =>
            options.UseMySql(
                  builder.Configuration.GetConnectionString("DefaultConnection"),
                  ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                   ));
            //add identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<EcoPowerDbContext>()
             .AddDefaultTokenProviders();
            //add Authentication 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.SaveToken = false;
                op.RequireHttpsMetadata = false;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            //inject automapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //inject services 
            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
         // builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IPackageRepository, PackageRepository>();

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<CloudinaryService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            //seed roles 
           using(var scope = app.Services.CreateScope())
            {
                var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRoles.SeedRolesAsync(rolemanager);
            }
            app.Run();
        }
    }
}
