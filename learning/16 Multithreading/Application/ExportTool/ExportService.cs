using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using BankingSystemServices;
using CsvHelper.TypeConversion;
using Services.Exceptions;

namespace ExportTool;

public class ExportService
{
    
    private readonly TypeConverterOptions _dateTimeOptions = new()
    {
        Formats = new[] { "yyyy-MM-ddTHH:mm:ssZ" },
        DateTimeStyle = DateTimeStyles.AdjustToUniversal
    };
    
    public void WriteClientsDataToScvFile(List<Client> clients, string pathToDirectory, string csvFileName)
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
            throw new CustomException("Неверно переданы параметры для считывания данных.");
        
        var fullPath = GetFullPathToFile(pathToDirectory, csvFileName);
        
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
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

    private string GetFullPathToFile(string pathToDirectory, string csvFileName)
    {
        return Path.Combine(pathToDirectory, csvFileName);
    }
}