namespace lab3SO;

public class Warehouse
{
    public enum ItemStatus { Available, PreOrder, Unavailable }

    public async Task<ItemStatus> FetchBookStatusAsync(string bookTitle)
    {
        await Task.Delay(2000); // Имитация задержки в 2 секунды
        var random = new Random();
        int idx = random.Next(0, Enum.GetValues(typeof(ItemStatus)).Length);
        return (ItemStatus)idx;
    }
}

public class Bookstore
{
    public static async Task Main(string[] args)
    {
        var warehouse = new Warehouse();
        var titles = new List<string>
        {
            "Harry Potter and the Philosopher's Stone",
            "Harry Potter and the Chamber of Secrets",
            "Harry Potter and the Prisoner of Azkaban",
            "Harry Potter and the Goblet of Fire",
            "Harry Potter and the Half-Blood Prince",
            "Harry Potter and the Deathly Hallows"
        };

        var timeBeforeFirstFetch = DateTime.Now;
        foreach (var title in titles)
        {
            Console.WriteLine($"{title} - {await warehouse.FetchBookStatusAsync(title)}");
        }

        Console.WriteLine($"Time elapsed {(DateTime.Now - timeBeforeFirstFetch).Milliseconds}ms");
    }
}