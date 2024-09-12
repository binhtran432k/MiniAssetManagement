using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using MiniAssetManagement.UseCases.Users;
using MiniAssetManagement.UseCases.Users.List;
using MiniAssetManagement.Web.Constants;

namespace MiniAssetManagement.Web.Users;

public class List(IMediator mediator) : Endpoint<ListUsersRequest>
{
    public override void Configure()
    {
        Get(ListUsersRequest.Route);
        AllowAnonymous(Http.GET);
        Summary(s =>
        {
            s.Summary = "List Users.";
        });

        Description(b =>
            b.Produces(StatusCodes.Status200OK, typeof(ListUsersResponse))
                .Produces(StatusCodes.Status500InternalServerError)
        );
    }

    public override async Task HandleAsync(
        ListUsersRequest request,
        CancellationToken cancellationToken
    )
    {
        int pageIndex = request.PageIndex ?? 0;
        (int? Skip, int? Take) pageQuery = request.PageSize is not null
            ? (pageIndex * request.PageSize, request.PageSize)
            : (null, null);
        var result = await mediator.Send(
            new ListUsersQuery(pageQuery.Skip, pageQuery.Take),
            cancellationToken
        );

        if (result.IsSuccess)
        {
            var (users, count) = result.Value;
            int pageCount =
                request.PageSize is not null
                    ? int.Parse(Math.Ceiling((decimal)count / (decimal)request.PageSize).ToString())
                : count > 0 ? 1
                : 0;

            await SendAsync(
                new ListUsersResponse(users, pageCount),
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

public class ListUsersRequest
{
    public const string Route = RouteConstant.User;

    [QueryParam]
    public int? PageIndex { get; init; }

    [QueryParam]
    public int? PageSize { get; init; }
}

public class ListUsersValidator : Validator<ListUsersRequest>
{
    public ListUsersValidator()
    {
        RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0);
    }
}

public record ListUsersResponse(IEnumerable<UserDTO> Items, int PageCount);
