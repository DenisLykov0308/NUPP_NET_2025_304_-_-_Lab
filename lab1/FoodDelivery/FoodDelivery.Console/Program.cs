using System.Text.Json;
using FoodDelivery.Common;

Console.WriteLine("=== Lab2: Parallel create + stats + save ===");

var service = new InMemoryCrudService<Courier>();

const int total = 1000;

Parallel.For(0, total, i =>
{
    var courier = Courier.CreateNew();

    courier.Rating = Random.Shared.NextDouble() * 5.0;     
    courier.IsAvailable = Random.Shared.Next(0, 2) == 1;     
    courier.TransportType = (i % 3) switch
    {
        0 => "Bike",
        1 => "Car",
        _ => "Foot"
    };

    service.Create(courier);
});

Console.WriteLine($"CREATE (Parallel): Додано {service.ReadAll().Count()} кур'єрів");

var couriers = service.ReadAll().ToList();

var minRating = couriers.Min(c => c.Rating);
var maxRating = couriers.Max(c => c.Rating);
var avgRating = couriers.Average(c => c.Rating);

var availableCount = couriers.Count(c => c.IsAvailable);

Console.WriteLine("\n=== Stats (Courier.Rating) ===");
Console.WriteLine($"Min: {minRating:F2}");
Console.WriteLine($"Max: {maxRating:F2}");
Console.WriteLine($"Avg: {avgRating:F2}");
Console.WriteLine($"Available: {availableCount}/{couriers.Count}");

var filePath = Path.Combine(AppContext.BaseDirectory, "data", "couriers.json");
Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

var json = JsonSerializer.Serialize(couriers, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText(filePath, json);

Console.WriteLine($"\nSaved to file: {filePath}");
Console.WriteLine("\n=== Done ===");
Console.ReadKey();
