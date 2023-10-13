using Microsoft.EntityFrameworkCore;
using OstLib;
using OstLib.Repository;
using OstLib.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(ac => ac.UseNpgsql(connection));

builder.Services.AddTransient<ITimeTable, TimeTableService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(e => e.MapControllers());

app.Run();
