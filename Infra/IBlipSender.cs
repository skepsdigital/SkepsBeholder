using RestEase;

namespace SkepsBeholder.Infra
{
    public interface IBlipSender
    {
        [Post("api/traces/{botid}")]
        Task<dynamic> SendContactAsync(
           [Path] string botid,
           [Body] string body,
           [Header("Content-Type")] string contentType = "application/json"
       );

    }
}
