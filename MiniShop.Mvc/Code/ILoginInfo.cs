namespace MiniShop.Mvc.Code
{
    public interface ILoginInfo
    {
        string AccessToken { get; }
        string RefreshToken { get; }
        string ExpiresIn { get; }
    }
}
