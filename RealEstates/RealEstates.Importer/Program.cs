using System.Text.Json;
using RealEstates.Data;
using RealEstates.Services;

class Program
{
    static void Main()
    {
        ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
        Console.WriteLine();
        ImportJsonFile("imot.bg-raw-data-2021-03-18.json");
    }

    private static void ImportJsonFile(string fileName)
    {
        var dbContext = new ApplicationDbContext();
        IPropertiesService propertiesService = new PropertiesService(dbContext);
        var propperties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>
            (File.ReadAllText(fileName));

        foreach (var jsonProp in propperties)
        {
            propertiesService.Add(jsonProp.District, jsonProp.Price, jsonProp.Floor,
                jsonProp.TotalFloors, jsonProp.Size, jsonProp.YardSize,
                jsonProp.Year, jsonProp.Type, jsonProp.BuildingType);
            Console.Write(".");
        }
    }
}
