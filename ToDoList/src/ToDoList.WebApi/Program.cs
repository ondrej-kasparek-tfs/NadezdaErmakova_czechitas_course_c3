using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddSingleton<IRepository<ToDoItem>, ToDoItemsRepository>();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}

app.MapGet("/nazdarSvete", () => "Ahoj, kouc Ondra!");

app.Run();
