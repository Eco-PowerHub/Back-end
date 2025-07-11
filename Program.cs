using Prometheus;
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
using Hangfire;
using Hangfire.MySql;
using System.Data;
using System.Transactions;


namespace EcoPowerHub
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            
            #region Email config
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddHttpContextAccessor();
            #endregion

            #region Email Templates
            builder.Services.AddSingleton<EmailTemplateService>(provider =>
            new EmailTemplateService(Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "WelcomeEmailTemplate.html")));
            #endregion

            #region ConnectionString 
            builder.Services.AddDbContext<EcoPowerDbContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            
            #region Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<EcoPowerDbContext>()
          .AddDefaultTokenProviders();
            #endregion

            #region Add Authentication
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
            #endregion
            
            #region Add Authorization Policy
            builder.Services.AddAuthorization(options =>
               {
                   options.AddPolicy("Company and Admin", policy => policy.RequireRole("Company", "Admin"));
                   options.AddPolicy("Only Client", policy => policy.RequireRole("Client"));
                   options.AddPolicy("Only Admin", policy => policy.RequireRole("Admin"));
                   options.AddPolicy("Client and Admin", policy => policy.RequireRole("Client", "Admin"));
                   options.AddPolicy("Client and Company", policy => policy.RequireRole("Client", "Company"));
               });
            #endregion

            
            #region Add Authorization Headers
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
            #endregion


            #region Inject Automapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            #endregion

            #region  Inject Services 

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddTransient<IEmailService, EmailService>(); 
            #endregion


            builder.Services.AddLogging();
            builder.Logging.AddConsole();
            builder.Services.AddControllers();

            #region Add Cores

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
            #endregion


            //builder.Services.AddScoped<BackgroundJobService>();
            //// Add Hangfire with MySQL storage
            //builder.Services.AddHangfire(config => config
            //    .UseStorage(new MySqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlStorageOptions
            //    {
            //        TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
            //        QueuePollInterval = TimeSpan.FromSeconds(15), // Adjust based on workload
            //        JobExpirationCheckInterval = TimeSpan.FromHours(1),
            //        TablesPrefix = "Hangfire_" // Prefix for Hangfire tables
            //    }))
            //);

            #region Cloudinary DI
            builder.Services.Configure<CloudinarySettings>(
             builder.Configuration.GetSection("CloudinarySettings"));
            var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            if (cloudinarySettings == null ||
                string.IsNullOrEmpty(cloudinarySettings.CloudName) ||
                string.IsNullOrEmpty(cloudinarySettings.ApiKey) ||
                string.IsNullOrEmpty(cloudinarySettings.ApiSecret))
            {
                throw new InvalidOperationException("Cloudinary settings are not properly configured.");
            }
            var account = new Account(
            cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret
            );
            var cloudinary = new Cloudinary(account);

            builder.Services.AddSingleton(cloudinary);
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>(); 
            #endregion

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            #region MiddleWares
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            //   app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            // Prometheus Exporter to collect Metrcis from /metrics Endpoint
            app.UseHttpMetrics();      // Tracks HTTP request metrics
            app.UseMetricServer();     // Exposes the /metrics endpoint

            #endregion

            #region Seed Roles 

            using (var scope = app.Services.CreateScope())
            {
                var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRoles.SeedRolesAsync(rolemanager);
            } 
            #endregion

            app.Run();
        }
    }
}
