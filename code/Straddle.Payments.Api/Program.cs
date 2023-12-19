namespace Straddle.Payments.Api;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Straddle.Payments.Api.Security;
using Straddle.Payments.Application;
using Straddle.Payments.Domain.Services;
using Straddle.Payments.Infrastructure;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddPaymentsApplication();
        InfrastructureOptions infrastructureOptions = new();
        builder.Configuration.Bind("Infrastructure", infrastructureOptions);
        builder.Services.AddPaymentsInfrastructure(infrastructureOptions);
        builder.Services.AddScoped<IUserProvider, HttpUserProvider>();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}