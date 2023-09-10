namespace CacheTests
{
    public class UnitTest1
    {
        [Fact]
        public void One_Element_Added()
        {
            LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
            cacheTester.AddElement("Cache Element 1");
            bool result = (cacheTester.memCache.Count == 1);
            cacheTester.EvictAll();
            Assert.True(result, "One element added in the cache");
        }

        [Fact]
        public void Two_Elements_Added()
        {
            LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
            cacheTester.AddElement("Cache Element 1");
            cacheTester.AddElement("Cache Element 2");
            bool result = (cacheTester.memCache.Count == 1);
            cacheTester.EvictAll();
            Assert.False(result, "More than one element added in the cache");
        }

        [Fact]
        public void Element_Retrieved()
        {
            LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
            cacheTester.AddElement("Cache Element 1");
            cacheTester.AddElement("Cache Element 2");
            bool result = (cacheTester.RetrieveElement(2) != null);
            cacheTester.EvictAll();
            Assert.True(result, "Element not retrieved");
        }

        [Fact]
        public void Element_Not_Retrieved()
        {
            LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
            cacheTester.AddElement("Cache Element 1");
            cacheTester.AddElement("Cache Element 2");
            bool result = (cacheTester.RetrieveElement(3) == null);
            cacheTester.EvictAll();
            Assert.True(result, "Element retrieved and it shouldn't");
        }

        [Fact]
        public void Least_Element_Replaced()
        {
            LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
            cacheTester.AddElement("Cache Element 1");
            cacheTester.AddElement("Cache Element 2");
            cacheTester.AddElement("Cache Element 3");
            cacheTester.AddElement("Cache Element 4");
            cacheTester.AddElement("Cache Element 5");
            bool result = (cacheTester.memCache.Count == 4);
            cacheTester.EvictAll();
            Assert.True(result, "Element retrieved");
        }

        [Fact]
        public void Evict_All()
        {
            LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
            cacheTester.AddElement("Cache Element 1");
            Thread.Sleep(1000);
            cacheTester.AddElement("Cache Element 2");
            cacheTester.AddElement("Cache Element 3");
            cacheTester.AddElement("Cache Element 4");
            cacheTester.AddElement("Cache Element 5");
            cacheTester.EvictAll();
            bool result = (cacheTester.memCache.Count == 0);
            Assert.True(result, "Element retrieved");
        }
    }
}