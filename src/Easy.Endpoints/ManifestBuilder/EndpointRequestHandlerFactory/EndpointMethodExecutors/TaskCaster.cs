using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class TaskCaster<T>
    {
        public static async Task<object?> Cast(Task<T> source) => (object?)await source.ConfigureAwait(false);

    }


}
