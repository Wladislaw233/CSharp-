using System.Globalization;
using System.Text;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;

namespace BankingSystemServices.ExportTool;

public class ExportService
{
    private static readonly TypeConverterOptions DateTimeOptions = new()
    {
        Formats = new[] { "yyyy-MM-ddTHH:mm:ssZ" },
        DateTimeStyle = DateTimeStyles.AdjustToUniversal
    };

    public static void WriteClientsDataToScvFile(List<Client> clients, string pathToDirectory, string csvFileName)
    {
        if (string.IsNullOrWhiteSpace(pathToDirectory) || string.IsNullOrWhiteSpace(csvFileName))
            throw new CustomException("Неверно переданы параметры для записи данных.");

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
                    csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime>(DateTimeOptions);
                    csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(DateTimeOptions);
                    csvWriter.WriteRecords(clients);
                    csvWriter.Flush();
                }
            }
        }
    }

    public static List<Client> ReadClientsDataFromScvFile(string pathToDirectory, string csvFileName)
    {
        if (string.IsNullOrWhiteSpace(pathToDirectory) || string.IsNullOrWhiteSpace(csvFileName))
            throw new CustomException("Неверно переданы параметры для считывания данных.");

        var fullPath = GetFullPathToFile(pathToDirectory, csvFileName);

        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            using (var streamReader = new StreamReader(fileStream))
            {
                using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime>(DateTimeOptions);
                    csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(DateTimeOptions);
                    csvReader.Read();
                    csvReader.ReadHeader();
                    return csvReader.GetRecords<Client>().ToList();
                }
            }
        }
    }

    public static void WritePersonsDataToJsonFile<T>(T persons, string pathToDirectory, string jsonFileName)
    {
        var directoryInfo = new DirectoryInfo(pathToDirectory);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        var fullPath = GetFullPathToFile(pathToDirectory, jsonFileName);
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            var jsonTextBytesArray = Encoding.Default.GetBytes(JsonConvert.SerializeObject(persons));
            fileStream.Write(jsonTextBytesArray, 0, jsonTextBytesArray.Length);
        }
    }

    public static List<T> ReadPersonsDataFromJsonFile<T>(string pathToDirectory, string jsonFileName)
    {
        var fullPath = GetFullPathToFile(pathToDirectory, jsonFileName);

        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            var jsonTextBytesArray = new byte[fileStream.Length];
            fileStream.Read(jsonTextBytesArray, 0, jsonTextBytesArray.Length);

            var jsonText = Encoding.Default.GetString(jsonTextBytesArray);
            var persons = JsonConvert.DeserializeObject<T[]>(jsonText);
            if (persons != null)
                return persons.ToList();
            return new List<T>();
        }
    }

    private static string GetFullPathToFile(string pathToDirectory, string fileName)
    {
        return Path.Combine(pathToDirectory, fileName);
    }
}