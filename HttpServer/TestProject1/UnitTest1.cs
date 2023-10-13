using System.Collections;
using Flurl;
using Flurl.Http;
using HttpServer.Models;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup() {}

    [Test]
    public async Task Test1()
    {
        IEnumerable<TimeTableEntryModel> list = await "http://localhost:8759/TimeTable".GetJsonAsync<IEnumerable<TimeTableEntryModel>>();
        Console.WriteLine(list);
        Assert.Pass();
    }
}
