using System.Threading.Tasks;
using Refit;

namespace RefitSample
{
    public interface IHelloClient
    {
        [Get("/helloworld")]
        Task<Reply> GetMessageAsync();
    }

    public class Reply
    {
        public string Message { get; set; }
    }
}
