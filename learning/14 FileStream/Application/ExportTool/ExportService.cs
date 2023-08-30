﻿using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using BankingSystemServices;
using CsvHelper.TypeConversion;

namespace ExportTool;

public class ExportService
{
    private string PathToDirectory { get; set; }
    private string CsvFileName { get; set; }

    private readonly TypeConverterOptions _dateTimeOptions = new()
    {
        Formats = new[] { "yyyy-MM-ddTHH:mm:ssZ" },
        DateTimeStyle = DateTimeStyles.AdjustToUniversal
    };

    public ExportService(string pathToDirectory, string csvFileName)
    {
        PathToDirectory = pathToDirectory;
        CsvFileName = csvFileName;
    }

    public void WriteClientsDataToScvFile(List<Client> clients)
    {
        var directoryInfo = new DirectoryInfo(PathToDirectory);
        
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        
        var fullPath = GetFullPathToFile();
        
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

    public List<Client> ReadClientsDataFromScvFile()
    {
        var fullPath = GetFullPathToFile();
        
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

    private string GetFullPathToFile()
    {
        return Path.Combine(PathToDirectory, CsvFileName);
    }
}