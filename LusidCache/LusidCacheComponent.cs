using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace LusidCache
{
    public class LusidCacheComponent
    {
        private static long cacheSizeLimit = 0;

        public static Lazy<LusidCacheComponent> lazyLusidCacheComponent = new Lazy<LusidCacheComponent>(() => new LusidCacheComponent());

        private LusidCacheComponent() { }
       
        public static LusidCacheComponent GetInstance
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                     .AddJsonFile($"appsettings.json");
                var config = configuration.Build();
                if (long.TryParse(config.GetSection("CacheSizeLimit").Value, out cacheSizeLimit))
                    return lazyLusidCacheComponent.Value;
                else
                    throw new Exception("Configuration not available to read limit of cache size");
            }
        }

        /// <summary>
        /// Memory cache object with the size read in configuration
        /// </summary>
        public MemoryCache memCache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
                TrackStatistics = true,
                SizeLimit = cacheSizeLimit
            });

        /// <summary>
        /// Adds an element to the cache for empty spaces or creating space using eviction
        /// </summary>
        /// <param name="element">Element to be added</param>
        public void AddElement (object element)
        {
            try
            {
                if (memCache.Count < cacheSizeLimit)
                {
                    AddElement(element, memCache.Count + 1);
                }
                else
                {
                    //Add an element replacing the least recently used
                    AddElement(element, EvictUsedLast());
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds and element to the cache
        /// </summary>
        /// <param name="element">Element to add</param>
        /// <param name="key">Key where the element is added</param>
        private void AddElement(object element, int key)
        {
            // All entries added to the cache have size 1 and a evicting message is sent if the element is evicted
            memCache.Set(key, element, new MemoryCacheEntryOptions()
                .SetSize(1)
                .RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    Console.WriteLine($"Cache entry with key {key} and value {value} was evicted due to {reason}");
                }, state: null)
            );
        }

        /// <summary>
        /// Evict the least recently used with compact functionality
        /// </summary>
        /// <returns>Returns the key evicted</returns>
        public int EvictUsedLast()
        {
            try
            {
                //Calculation of the percentage to evict depending on the cache size and knowing that we only want to evict one element
                double compactOneEntry = (100.00/(double)cacheSizeLimit)/100.00;

                memCache.Compact(compactOneEntry);

                for (int i = 1; i <= cacheSizeLimit; i++)
                {
                    if (memCache.Get(i) == null)
                        return i;
                }

                //If we don't find a null element, compact didn't work and program needs to be restarted
                throw new Exception("Cache is non functional, restart system");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Clears teh cache completely
        /// </summary>
        public void EvictAll()
        {
            try
            {
                memCache.Clear();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve element from cache based on its key
        /// </summary>
        /// <param name="key">Key of the element to retrieve</param>
        /// <returns>Element from teh cache</returns>
        public object? RetrieveElement(int key)
        {
            try
            {
                Object? result = memCache.Get(key);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    //Search for the value in data origin and potentially adding it to cache
                    return null;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}