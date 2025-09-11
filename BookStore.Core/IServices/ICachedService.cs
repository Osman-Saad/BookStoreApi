namespace BookStore.Core.IServices
{
    public interface ICachedService
    {

        public Task CacheResponse(string key, object response, TimeSpan timeSpan);
        public Task<string> GetCashedResponse(string key);
    }
}
