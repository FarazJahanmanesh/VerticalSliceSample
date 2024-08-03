using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Sample.Api.Data;
using Sample.Api.Entities;

namespace Sample.Api.Features.Books;

//public static class GetAllBooks
//{
//    public static void AddEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("api/books",async(ISender sender) =>
//        {
//            var books = await sender.Send(new GetAllBooksCommand());

//            return Results.Ok(books);
//        });
//    }
//}

public record GetAllBooksCommand:IRequest<List<Book>>;

internal class GetAllBooksHandler : IRequestHandler<GetAllBooksCommand, List<Book>>
{
    private readonly AppDbContext _dbContext;
    public GetAllBooksHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<List<Book>> Handle(GetAllBooksCommand request, CancellationToken cancellationToken)
    {
        var books = _dbContext.Books.AsNoTracking().ToList();

        return Task.FromResult(books);
    }
}
public class GetAllBooksEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/books", async (ISender sender) =>
        {
            var books = await sender.Send(new GetAllBooksCommand());

            return Results.Ok(books);
        });
    }
}
