using SkepsBeholder.Model;

namespace SkepsBeholder.Services.Interfaces
{
    public interface IProcess
    {
        Task ProcessLogAsync(Log log);
    }
}
