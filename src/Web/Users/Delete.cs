using Ardalis.Result;
using FastEndpoints;
using MediatR;
using MiniAssetManagement.UseCases.Users.Delete;
using MiniAssetManagement.Web.Constants;

namespace MiniAssetManagement.Web.Users;

public class Delete(IMediator mediator) : Endpoint<DeleteRequest>
{
    public override void Configure()
    {
        Delete(DeleteRequest.Route);
        AllowAnonymous(Http.DELETE);
        Summary(s =>
        {
            s.Summary = "Delete an existing User.";
        });
        Description(b =>
            b.Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ClearDefaultProduces([StatusCodes.Status200OK])
        );
    }

    public override async Task HandleAsync(
        DeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(new DeleteUserCommand(request.UserId), cancellationToken);

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

public class DeleteRequest
{
    public static readonly string Route = RouteConstant.BuildUserById("{UserId:int}");

    public int UserId { get; init; }
}
