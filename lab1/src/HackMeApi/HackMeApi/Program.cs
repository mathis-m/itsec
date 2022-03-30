using HackMeApi.Infrastructure;
using HackMeApi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;

#if DEBUG
var uiPath = "../../ui";
#else
var uiPath = "ui";
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<PostService>();
builder.Services.AddDbContext<HackMeContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:3000");
    });
});

builder.Services
    .AddSpaStaticFiles(config => { config.RootPath = uiPath; });


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // make cookie accessable via js
        options.Cookie.HttpOnly = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if(app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.UseCors("ClientPermission");


app.UseSpaStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});



app.UseSpa(spa =>
{
    spa.Options.SourcePath = uiPath;
#if DEBUG
    spa.UseReactDevelopmentServer(npmScript: "start");
#endif
});

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    for (var i = 0; i < 10; i++)
    {
        logger.LogInformation("Try connect to database...");
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<HackMeContext>();
            db.Database.Migrate();
            break;
        }
        catch
        {
            logger.LogWarning("Error while db setup, retry in 2s");
            if (i == 9)
            {
                throw;
            }

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}



app.Run();