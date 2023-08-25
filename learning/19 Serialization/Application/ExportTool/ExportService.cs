using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Models;
using Newtonsoft.Json;

namespace ExportTool;

public class ExportService
{
    private string PathToDirectory { get; set; }
    private string FileName { get; set; }

    public ExportService(string pathToDirectory, string csvFileName)
    {
        PathToDirectory = pathToDirectory;
        FileName = csvFileName;
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
                    csvWriter.Context.RegisterClassMap<ClientMapper>();
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
                    csvReader.Context.RegisterClassMap<ClientMapper>();
                    csvReader.Read();
                    csvReader.ReadHeader();
                    return csvReader.GetRecords<Client>().ToList();
                }
            }
        }
    }

    public void WritePersonsDataToJsonFile<T>(T persons, string fileName)
    {
        FileName = fileName;
        var directoryInfo = new DirectoryInfo(PathToDirectory);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        var fullPath = GetFullPathToFile();
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            var jsonTextBytesArray = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(persons));
            fileStream.Write(jsonTextBytesArray,0, jsonTextBytesArray.Length);
        }
    }

    public List<T> ReadPersonsDataFromJsonFile<T>(string fileName)
    {
        FileName = fileName;
        var fullPath = GetFullPathToFile();
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            var jsonTextBytesArray = new byte[fileStream.Length];
            
            fileStream.Read(jsonTextBytesArray, 0, jsonTextBytesArray.Length);
            var jsonText = System.Text.Encoding.Default.GetString(jsonTextBytesArray);
            var persons = JsonConvert.DeserializeObject<T[]>(jsonText);
            if (persons != null)
                return persons.ToList();
            else
                return new List<T>();
        }
    }
    private string GetFullPathToFile()
    {
        return Path.Combine(PathToDirectory, FileName);
    }
}