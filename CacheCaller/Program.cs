try
{
    //Call the GetInstance static method to get the Singleton Instance
    LusidCache.LusidCacheComponent cacheTester = LusidCache.LusidCacheComponent.GetInstance;
    cacheTester.AddElement("Cache Element 1");
    cacheTester.AddElement("Cache Element 2");
    cacheTester.AddElement("Cache Element 3");
    cacheTester.AddElement("Cache Element 4");

    object? cacheResult = cacheTester.RetrieveElement(1);
    Console.WriteLine(cacheResult.ToString());

    cacheResult = cacheTester.RetrieveElement(4);
    Console.WriteLine(cacheResult.ToString());

    cacheTester.AddElement("Cache Element 5");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}