
namespace SolicitorSearch.Utilities.HttpAccess.Interfaces
{
    public interface IHttpProxy
    {
        Task<string> GetHtmlResponseAsync(string url, CancellationToken cancellationToken);
    }
}
