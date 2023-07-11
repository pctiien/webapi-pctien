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

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
