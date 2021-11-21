using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class ValueTaskCaster<T>
    {
        public static async Task<object?> Cast(ValueTask<T> source) => await source.ConfigureAwait(false);

    }


}
