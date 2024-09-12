using FastEndpoints;
using FluentValidation;
using MediatR;
using MiniAssetManagement.Infrastructure.Data.Config;
using MiniAssetManagement.UseCases.Users.Create;
using MiniAssetManagement.Web.Constants;

namespace MiniAssetManagement.Web.Users;

public class Create(IMediator mediator) : Endpoint<CreateUserRequest>
{
    public override void Configure()
    {
        Post(CreateUserRequest.Route);
        AllowAnonymous(Http.POST);
        Summary(s =>
        {
            s.Summary = "Create a new User.";
            s.ExampleRequest = new CreateUserRequest { Username = "binhtran" };
        });
        Description(b =>
            b.Produces(StatusCodes.Status201Created, typeof(CreateUserResponse))
                .Produces(StatusCodes.Status500InternalServerError)
                .ClearDefaultProduces([StatusCodes.Status200OK])
        );
    }

    public override async Task HandleAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(
            new CreateUserCommand(request.Username),
            cancellationToken
        );

        if (result.IsSuccess)
        {
            await SendAsync(
                new CreateUserResponse(result.Value, request.Username),
                StatusCodes.Status201Created,
                cancellationToken
            );
            return;
        }
    }
}

public class CreateUserRequest
{
    public const string Route = RouteConstant.User;

    public string Username { get; init; } = String.Empty;
}

public class CreateUserValidator : Validator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(2)
            .MaximumLength(DataSchemaConstants.DEFAULT_USERNAME_LENGTH);
    }
}

public record CreateUserResponse(int Id, string Username);