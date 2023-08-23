using CsvHelper.Configuration;

namespace Models;

public class ClientMapper : ClassMap<Client>
{
    public ClientMapper()
    {
        Map(client => client.FirstName).Index(0).Name("firstName");
        Map(client => client.LastName).Index(1).Name("lastName");
        Map(client => client.DateOfBirth).Index(2).Name("dateOfBirth");
        Map(client => client.Age).Index(3).Name("age");
        Map(client => client.ClientId).Index(4).Name("clientId");
        Map(client => client.PhoneNumber).Index(5).Name("phoneNumber");
        Map(client => client.Email).Index(6).Name("email");
        Map(client => client.Address).Index(7).Name("address");
    }
}