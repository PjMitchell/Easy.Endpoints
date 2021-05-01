using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Books
{
    public class GetBookEndpointHandler : IJsonResponseEndpointHandler<Book[]>
    {
        public Task<Book[]> Handle(CancellationToken cancellationToken) => Task.FromResult(AllBooks().ToArray());
        public static IEnumerable<Book> AllBooks()
        {
            return new[]
            {
                new Book { Id = 1, Name = "Book 1" },
                new Book { Id = 2, Name = "Book 2" },
            };
        }
    }
}
