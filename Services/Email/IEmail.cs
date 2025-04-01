
namespace SkepsBeholder.Services.Email
{
    public interface IEmail
    {
        Task SendMessageAsync(string recipient, string content);
    }
}
