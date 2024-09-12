namespace MiniAssetManagement.UseCases.Users.List;

public interface IListUsersQueryService
{
    Task<(IEnumerable<UserDTO>, int)> ListAsync(int? skip = null, int? take = null);
}