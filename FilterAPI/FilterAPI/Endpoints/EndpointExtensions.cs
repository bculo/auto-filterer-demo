using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Bogus;
using Bogus.DataSets;
using FilterAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Person = FilterAPI.Entities.Person;

namespace FilterAPI.Endpoints;

public record SeedDto(int NumberOfRecords);

[PossibleSortings("LastName", "FirstName")]
public class FilterPersonDto : PaginationFilterBase
{
    [CompareTo("LastName", "FirstName")]
    [StringFilterOptions(StringFilterOption.Contains)]
    public string? Search { get; set; }
    
    [CompareTo("Age")]
    public Range<int> AgeRange { get; set; }
}

public record FilterPersonResponseDto(string FirstName, string LastName, int Age, DateTime BornOn);


public static class EndpointExtensions
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapPost("/seed", async (FilterDbContext context, SeedDto request, CancellationToken ct) =>
            {
                var from = DateTime.UtcNow.AddYears(-50);
                var to = DateTime.UtcNow.AddYears(-18);
                
                var generator = new Faker<Person>()
                    .RuleFor(o => o.Age, f => f.Random.Int(18, 50))
                    .RuleFor(o => o.FirstName, f => f.Name.FirstName(Name.Gender.Male))
                    .RuleFor(o => o.LastName, f => f.Name.LastName(Name.Gender.Male))
                    .RuleFor(o => o.BornOn, f => f.Date.Between(from, to));

                var users = Enumerable.Range(0, request.NumberOfRecords)
                    .Select(x => generator.Generate())
                    .ToList();

                await context.Data.AddRangeAsync(users, ct);
                await context.SaveChangesAsync(ct);
            })
            .WithName("seed")
            .WithOpenApi();
        
        app.MapDelete("/delete-all", async (FilterDbContext context, CancellationToken ct) =>
            {
                await context.Data.ExecuteDeleteAsync(ct);
                await context.SaveChangesAsync(ct);
            })
            .WithName("delete-all")
            .WithOpenApi();
        

        app.MapPost("/filter", async ([AsParameters] FilterPersonDto request, FilterDbContext context, CancellationToken ct) =>
            {
                return await context.Data.ApplyFilter(request)
                    .Select(i => new FilterPersonResponseDto(i.FirstName, i.LastName, i.Age, i.BornOn))
                    .ToListAsync(ct);
            })
            .WithName("Seed")
            .WithOpenApi();

    }
}