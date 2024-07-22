using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connectionString);
builder.Services.AddScoped<GameStoreContext>();

var app = builder.Build();

GamesEndpoints.MapGamesEndPoints(app);
app.MapGenresEndPoints();

await app.MigrateDb();

app.Run();
