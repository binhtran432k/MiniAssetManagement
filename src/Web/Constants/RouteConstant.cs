namespace MiniAssetManagement.Web.Constants;

public static class RouteConstant
{
    public const string User = "/users";

    public static string BuildUserById(object id) => String.Format("{0}/{1}", User, id);

    public static string BuildUserUsernameById(object id) =>
        String.Format("{0}/{1}/username", User, id);
}