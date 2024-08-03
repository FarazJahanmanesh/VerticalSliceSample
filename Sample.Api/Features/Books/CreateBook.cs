using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Sample.Api.Data;
using Sample.Api.Entities;

namespace Sample.Api.Features.Books;

//public static class CreateBook
//{
//    public static void AddEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("api/books", async (CreateBookCommand request, ISender sender) =>
//        {
//            var bookId = await sender.Send(request);

//            return Results.Ok(bookId);
//        });
//    }
//}

public record CreateBookCommand(string Name ,string Description) :IRequest<long>;

public class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty();

        RuleFor(d => d.Description)
            .NotEmpty();
    }
}

internal class CreateBookHandler : IRequestHandler<CreateBookCommand, long>
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<CreateBookCommand> _validator;
    public CreateBookHandler(AppDbContext dbContext, IValidator<CreateBookCommand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    public Task<long> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new BadHttpRequestException(string.Join(Environment.NewLine,validationResult.Errors.Select(d=>d.ErrorMessage)));
        }

        var book = new Book
        {
            Name = request.Name,
            Description = request.Description,
        };
        _dbContext.Books.Add(book);
        _dbContext.SaveChanges();

        return Task.FromResult(book.Id);
    }
}

public class CreateBookEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/books", async (CreateBookCommand request, ISender sender) =>
        {
            var bookId = await sender.Send(request);

            return Results.Ok(bookId);
        });
    }
}