﻿using GameStore.Data;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;
public static class GenresEndPoints
{
    public static RouteGroupBuilder MapGenresEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                .Select(genre => genre.ToDto())
                .AsNoTracking()
                .ToListAsync());

        return group;
    }
}
