using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games =
    [
    new (1, "The Witcher 3", "RPG", 29.99m, new DateOnly(2015, 5, 19)),
    new (2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new (3, "Doom Eternal", "FPS", 39.99m, new DateOnly(2020, 3, 20))
    ];

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();


        //GET / games
        group.MapGet("/", async (GameStoreContext dbContext) =>
         await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .AsNoTracking()
                .ToListAsync()
                );


        // GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(game.ToGameDetailsDto()) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);



        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
         {
             if (string.IsNullOrEmpty(newGame.Name)) return Results.BadRequest("Name is required.");

             Game game = newGame.ToEntity();

             dbContext.Games.Add(game);
             await dbContext.SaveChangesAsync();


             return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
         });



        // PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null) return Results.NotFound();

            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();
            return Results.NoContent();

        });



        // DELETE /games
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //games.RemoveAt(id - 1);
            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }

}
