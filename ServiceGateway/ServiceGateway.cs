using Blazored.LocalStorage;

namespace ServiceGateway;

public class ServiceGateway
{
    public UserService.UserService UserService;

    public ServiceGateway(ISyncLocalStorageService localStorage)
    {
        UserService = new(localStorage);
    }
}
