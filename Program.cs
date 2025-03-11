
using CloudinaryDotNet;
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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;


namespace EcoPowerHub
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            // Add services to the container.

            //Email config
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<EmailTemplateService>(provider =>
            new EmailTemplateService(Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "WelcomeEmailTemplate.html")));
            //add ConnectionString 
            builder.Services.AddDbContext<EcoPowerDbContext>(options =>
                 options.UseMySql(
                 builder.Configuration.GetConnectionString("DefaultConnection"),
                 new MySqlServerVersion(new Version(8, 0, 41)) 
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
                op.SaveToken = true;
                op.RequireHttpsMetadata = false;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            //add authorization policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Company and Admin", policy => policy.RequireRole("Company", "Admin"));
                options.AddPolicy("Only Client", policy => policy.RequireRole("Client"));
                options.AddPolicy("Only Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Client and Admin", policy => policy.RequireRole("Client", "Admin"));
                options.AddPolicy("Client and Company", policy => policy.RequireRole("Client", "Company"));
            });

            //add authorization headers
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme , Id = "Bearer"}
                        },
                        new string []{}
                    }
                });
            });

            //cloudinary
            var cloudinarySettings = builder.Configuration.GetSection("Cloudinary").Get<CloudinarySettings>();
            var cloudinaryAccount = new Account(
                cloudinarySettings?.CloudName,
                cloudinarySettings?.ApiKey,
                cloudinarySettings?.ApiSecret
            );

            var cloudinary = new Cloudinary(cloudinaryAccount);

            //inject automapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //inject services 
            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSession();
            builder.Services.AddLogging();
            builder.Logging.AddConsole();

            builder.Services.AddControllers();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
         
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            //cloudinary DI
            builder.Services.AddSingleton(cloudinary);
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
            builder.Services.AddScoped<CloudinaryService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<CloudinaryService>();
            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.

            app.UseSwagger();
                app.UseSwaggerUI();

            //   app.UseHttpsRedirection();
            app.UseSession();
            app.UseRouting();
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
