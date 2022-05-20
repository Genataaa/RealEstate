﻿using System.Text;
using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        var db = new ApplicationDbContext();
        db.Database.Migrate();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Property search");
            Console.WriteLine("2. Most expensive districts");
            Console.WriteLine("3. Average price per square meter");
            Console.WriteLine("4. Average price per square meter for concretely district");
            Console.WriteLine("5. Add tag");
            Console.WriteLine("6. Bulk tag to poperties");
            Console.WriteLine("7. Property Full Info");
            Console.WriteLine("0. EXIT");
            bool parsed = int.TryParse(Console.ReadLine(), out int option);
            if (parsed && option == 0)
            {
                break;
            }

            if (parsed && option >= 1 && option <= 6)
            {
                switch (option)
                {
                    case 1:
                        PropertySearch(db);
                        break;
                    case 2:
                        MostExpensiveDistricts(db);
                        break;
                    case 3:
                        AveragePricePerSquareMeter(db);
                        break;
                    case 4:
                        AveragePricePerSquareMeterForConcretelyDistrict(db);
                        break;
                    case 5:
                        AddTag(db);
                        break;
                    case 6:
                        BulkTagToProperties(db);
                        break;
                    case 7:
                        PropertyFullInfo(db);
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private static void PropertyFullInfo(ApplicationDbContext db)
    {
        throw new NotImplementedException();
    }

    private static void BulkTagToProperties(ApplicationDbContext db)
    {
        throw new NotImplementedException();
    }

    private static void AddTag(ApplicationDbContext db)
    {
        Console.WriteLine("Tag name:");
        var name = Console.ReadLine();
        Console.WriteLine("Importance (optional):");
        bool isParsed = int.TryParse(Console.ReadLine(), out int tagImportance);
        int? importance = isParsed ? tagImportance : null;

        ITagService service = new TagService(db);
        service.AddTag(name, importance);
    }

    private static void AveragePricePerSquareMeter(ApplicationDbContext db)
    {
        IPropertiesService service = new PropertiesService(db);
        Console.WriteLine($"Average price per square meter: {service.AveragePricePerSquareMeter():0.00}€/m²");
    }

    private static void AveragePricePerSquareMeterForConcretelyDistrict(ApplicationDbContext db)
    {
        Console.WriteLine("PropertyId:");
        var propertyId = int.Parse(Console.ReadLine());
        IPropertiesService service = new PropertiesService(db);
        Console.WriteLine($"Average price per square meter: {service.AveragePricePerSquareMeter(propertyId):0.00}€/m²");
    }

    private static void MostExpensiveDistricts(ApplicationDbContext db)
    {
        Console.Write("Districts count:");
        int count = int.Parse(Console.ReadLine());

        IDistrictsService service = new DistrictsService(db);
        var districts = service.GetMostExpensiveDistricts(count);
        foreach (var district in districts)
        {
            Console.WriteLine($"{district.Name} => {district.AveragePricePerSquareMeter:0.00}€/m² ({district.PropertiesCount})");
        }
    }

    private static void PropertySearch(ApplicationDbContext db)
    {
        Console.Write("Min price:");
        int minPrice = int.Parse(Console.ReadLine());
        Console.Write("Max price:");
        int maxPrice = int.Parse(Console.ReadLine());
        Console.Write("Min size:");
        int minSize = int.Parse(Console.ReadLine());
        Console.Write("Max size:");
        int maxSize = int.Parse(Console.ReadLine());

        IPropertiesService service = new PropertiesService(db);
        var properties = service.Search(minPrice, maxPrice, minSize, maxSize);
        foreach (var property in properties)
        {
            Console.WriteLine($"{property.DistrictName}; {property.BuildingType}; {property.PropertyType} => {property.Price}€ => {property.Size}m²");
        }
    }
}
