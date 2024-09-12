using Ardalis.Result;
using FastEndpoints;
using MediatR;
using MiniAssetManagement.UseCases.Users.Get;
using MiniAssetManagement.Web.Constants;

namespace MiniAssetManagement.Web.Users;

public class GetById(IMediator mediator) : Endpoint<GetByIdUserRequest>
{
    public override void Configure()
    {
        Get(GetByIdUserRequest.Route);
        AllowAnonymous(Http.GET);
        Summary(s =>
        {
            s.Summary = "Get User By Id.";
        });
        Description(b =>
            b.Produces(StatusCodes.Status200OK, typeof(GetByIdUserResponse))
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
        );
    }

    public override async Task HandleAsync(
        GetByIdUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(new GetUserQuery(request.UserId), cancellationToken);

        if (result.IsSuccess)
        {
            await SendAsync(
                new GetByIdUserResponse(result.Value.Id, result.Value.Username),
                StatusCodes.Status200OK,
                cancellationToken
            );
            return;
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            await SendAsync(null, StatusCodes.Status404NotFound, cancellationToken);
            return;
        }
    }
}

public class GetByIdUserRequest
{
    public static readonly string Route = RouteConstant.BuildUserById("{UserId:int}");

    public int UserId { get; init; }
}

public record GetByIdUserResponse(int Id, string Username);