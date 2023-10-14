using Microsoft.EntityFrameworkCore;
using OstLib;
using OstLib.Repository;
using OstLib.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(ac => ac.UseNpgsql(connection));

builder.Services.AddTransient<ITimeTable, TimeTableService>();

builder.Services.AddControllers();

/*
 * TODO: Надобно сделать
 * 1. Контроллер для клиента:
 * 
 *    1.1 Метод для добавления клиента в базу
 *        На вход json с клиентом
 *        Ответ - ок или не ок
 * 
 *    1.2 Метод для получения клиента из базы по id.
 *        На вход json только с id клиента либо весь клиент целиком (подумать)
 * 
 * 2. Метод в TimeTableController либо отдельный контроллер:
 *    добавление записи в базу.
 *    На вход json с записью в расписание.
 *    Ответ - все ок или не ок
 */

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(e => e.MapControllers());

app.Run();
