using WebAppStarter.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Setup Application services
builder.Services.AddApplicationServices();

// Setup Infrastructures services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Setup Api services
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Ensure database is initialized
    await app.InitialiseDatabaseAsync();

    // Add swagger
    app.UseSwagger();
    app.UseSwaggerUI();

    // Setup cors
    app.UseCors("development");
}
else
{
    app.UseHttpsRedirection();
}

app.UseExceptionHandler(options => { });

app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("/index.html");

app.MapControllers();

app.Run();

public partial class Program
{
}
