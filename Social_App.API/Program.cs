using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Social_App.API.Config;
using Social_App.API.MediatRBehaviors;
using Social_App.API.Services;
using Social_App.API.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace Social_App.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IConversationService, ConversationService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSignalR();


            //CQRS Configurations

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            //Database Configuration
            #region Marten Configuration
            //builder.Services.AddMarten(config =>
            //   {
            //       config.Connection(builder.Configuration.GetConnectionString("devDB")!);
            //   }).UseLightweightSessions(); 
            #endregion

            builder.Services.AddDbContext<ApplicationContext>(optionsAction =>
            {
                optionsAction.UseNpgsql(builder.Configuration.GetConnectionString("devDB"));
            });

            //helpers

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.Configure<EmailDetails>(builder.Configuration.GetSection("emailSenderOptions"));
            builder.Services.Configure<JwtDetails>(builder.Configuration.GetSection("Jwt"));

            //Scopes

            //builder.Services.AddScoped<IUserManagerWithMarten, UserManagerWithMarten>();
            builder.Services.AddScoped<IAccountServices, AccountServices>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PublicPolicy",
                    builder => builder.AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin());
            });

          

            //Authentication

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Append("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken) &&
                                context.HttpContext.Request.Path.StartsWithSegments("/chatHub"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:JwtIssuer"],
                        ValidAudience = builder.Configuration["Jwt:JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtKey"]!)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };

                });

            builder.Services.ConfigSwagger();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("PublicPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
