using logger.Middleware;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//DI
{
    //Global exception
    builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
}
// Serilog Configuration
{
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    

    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//Middlewares
{
    // Global Exception Middleware Pipline
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    //Serilog Request Middleware
    app.UseSerilogRequestLogging();
}


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
