using System.Configuration;
using System.Text;
using api.Data;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder);


// Configure 
var app = builder.Build();
Configure(app);







static void ConfigureServices(WebApplicationBuilder builder)
{
    var services = builder.Services;
    services.AddControllers();
    services.AddDbContext<MyDbContext>(opt=>
    {
        opt.UseMySQL(builder.Configuration.GetConnectionString("MyDb"));
    });
    services.AddScoped<ICategoryRepository,CategoryRepository>();
    services.AddScoped<IHangHoaRepository,HangHoaRepository>();
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    // JWT ScretKey
    var secretKey = builder.Configuration.GetSection("AppSettings").GetSection("SecretKey").Value;
    var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt=>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                // Tu sinh token
                ValidateIssuer = false,
                ValidateAudience = false,
                // Ky vao token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero
            };
        });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}
static void Configure(WebApplication app)
{
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
