﻿using System.Globalization;
using System.Text;
using BankingSystemServices.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;

namespace BankingSystemServices.ExportTool;

public class ExportService
{
    private readonly TypeConverterOptions _dateTimeOptions = new()
    {
        Formats = new[] { "yyyy-MM-ddTHH:mm:ssZ" },
        DateTimeStyle = DateTimeStyles.AdjustToUniversal
    };

    public void WriteClientsDataToScvFile(IEnumerable<Client> clients, string pathToDirectory, string csvFileName)
    {
        if (string.IsNullOrWhiteSpace(pathToDirectory) || string.IsNullOrWhiteSpace(csvFileName))
            throw new ArgumentException("The parameters for writing data were passed incorrectly.");

        var directoryInfo = new DirectoryInfo(pathToDirectory);

        if (!directoryInfo.Exists)
            directoryInfo.Create();

        var fullPath = GetFullPathToFile(pathToDirectory, csvFileName);

        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            using (var streamWriter = new StreamWriter(fileStream))
            {
                using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime>(_dateTimeOptions);
                    csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(_dateTimeOptions);
                    csvWriter.WriteRecords(clients);
                    csvWriter.Flush();
                }
            }
        }
    }

    public List<Client> ReadClientsDataFromScvFile(string pathToDirectory, string csvFileName)
    {
        if (string.IsNullOrWhiteSpace(pathToDirectory) || string.IsNullOrWhiteSpace(csvFileName))
            throw new ArgumentException("The parameters for writing data were passed incorrectly.");

        var fullPath = GetFullPathToFile(pathToDirectory, csvFileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("The CSV file to read does not exist!");
        
        using (var fileStream = new FileStream(fullPath, FileMode.Open))
        {
            using (var streamReader = new StreamReader(fileStream))
            {
                using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime>(_dateTimeOptions);
                    csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(_dateTimeOptions);
                    csvReader.Read();
                    csvReader.ReadHeader();
                    return csvReader.GetRecords<Client>().ToList();
                }
            }
        }
    }

    public static void WritePersonsDataToJsonFile<T>(List<T> persons, string pathToDirectory, string jsonFileName)
        where T : Person
    {
        if (string.IsNullOrWhiteSpace(pathToDirectory) || string.IsNullOrWhiteSpace(jsonFileName))
            throw new ArgumentException("The parameters for writing data were passed incorrectly.");
        
        var directoryInfo = new DirectoryInfo(pathToDirectory);
        
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        
        var fullPath = GetFullPathToFile(pathToDirectory, jsonFileName);

        using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
        
        var jsonTextBytesArray = Encoding.Default.GetBytes(JsonConvert.SerializeObject(persons));
            
        fileStream.Write(jsonTextBytesArray, 0, jsonTextBytesArray.Length);
    }

    public static List<T> ReadPersonsDataFromJsonFile<T>(string pathToDirectory, string jsonFileName) where T : Person
    {
        if (string.IsNullOrWhiteSpace(pathToDirectory) || string.IsNullOrWhiteSpace(jsonFileName))
            throw new ArgumentException("The parameters for reading data were passed incorrectly.");
        
        var fullPath = GetFullPathToFile(pathToDirectory, jsonFileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("The JSON file to read does not exist!");

        using var fileStream = new FileStream(fullPath, FileMode.Open);
        
        var jsonTextBytesArray = new byte[fileStream.Length];
            
        // ReSharper disable once MustUseReturnValue
        fileStream.Read(jsonTextBytesArray, 0, jsonTextBytesArray.Length);

        var jsonText = Encoding.Default.GetString(jsonTextBytesArray);
            
        var persons = JsonConvert.DeserializeObject<T[]>(jsonText);
            
        return persons != null ? persons.ToList() : new List<T>();
    }

    private static string GetFullPathToFile(string pathToDirectory, string fileName)
    {
        return Path.Combine(pathToDirectory, fileName);
    }
}