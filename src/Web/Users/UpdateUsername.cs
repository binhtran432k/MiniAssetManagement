using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using MiniAssetManagement.Infrastructure.Data.Config;
using MiniAssetManagement.UseCases.Users.Update;
using MiniAssetManagement.Web.Constants;

namespace MiniAssetManagement.Web.Users;

public class UpdateUsername(IMediator mediator) : Endpoint<UpdateUsernameRequest>
{
    public override void Configure()
    {
        Put(UpdateUsernameRequest.Route);
        AllowAnonymous(Http.PUT);
        Summary(s =>
        {
            s.Summary = "Update a Username of an existing User.";
            s.ExampleRequest = new UpdateUsernameRequest { Value = "binhtran" };
        });
        Description(b =>
            b.Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ClearDefaultProduces([StatusCodes.Status200OK])
        );
    }

    public override async Task HandleAsync(
        UpdateUsernameRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(
            new UpdateUserCommand(request.UserId, request.Value),
            cancellationToken
        );

        if (result.IsSuccess)
        {
            await SendAsync(null, StatusCodes.Status204NoContent, cancellationToken);
            return;
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            await SendAsync(null, StatusCodes.Status404NotFound, cancellationToken);
            return;
        }
    }
}

public class UpdateUsernameRequest
{
    public static readonly string Route = RouteConstant.BuildUserUsernameById("{UserId:int}");

    public int UserId { get; init; }
    public string Value { get; init; } = String.Empty;
}

public class UpdateUsernameValidator : Validator<UpdateUsernameRequest>
{
    public UpdateUsernameValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Value is required.")
            .MinimumLength(2)
            .MaximumLength(DataSchemaConstants.DEFAULT_USERNAME_LENGTH);
    }
}
