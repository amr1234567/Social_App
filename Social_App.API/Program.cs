using Marten;
using Social_App.API.MediatRBehaviors;
using Social_App.Services.IdentityServices;
using Social_App.Services.Interfaces;
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


            //CQRS Configurations

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            //Database Configuration
            builder.Services.AddMarten(config =>
            {
                config.Connection(builder.Configuration.GetConnectionString("devDB")!);
            }).UseLightweightSessions();


            //Scopes

            builder.Services.AddScoped<IUserManagerWithMarten, UserManagerWithMarten>();
            builder.Services.AddScoped<IAccountServices, AccountServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
