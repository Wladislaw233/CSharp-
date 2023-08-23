using Services;
using Services.Database;

namespace ServiceTests;

public class MultithreadingTests
{
    public static void MultithreadingTest()
    {
        using var bankingSystemDbContext = new BankingSystemDbContext();
        var clientService = new ClientService(bankingSystemDbContext);
        var bankClients = TestDataGenerator.GenerateListWitchBankClients(1);
        var bankClient = bankClients.FirstOrDefault();
        if (bankClient != null)
        {
            // Добавление клиента и получение списка клиента.
            // Пока поток не добавит клиента, запретим получение списка клиентов.
            Parallel.Invoke(() =>
                {
                    clientService.AddClient(bankClient);
                    Console.WriteLine(
                        $"Добавляем клиента ID: {bankClient.ClientId}, {bankClient.FirstName} {bankClient.LastName}, дата рождения {bankClient.DateOfBirth.ToString("D")}");
                },
                () =>
                {
                    bankClients = clientService.ClientsWithFilterAndPagination(1, 100);
                    Console.WriteLine("Клиенты банка:" +
                                      "\n" + string.Join("\n",
                                          bankClients.Select(client =>
                                              $"ID {client.ClientId}, {client.FirstName} {client.LastName}, дата рождения {client.DateOfBirth.ToString("D")}")));
                }
            );

            // Начисление денег клиенту.
            // Пока один поток не начислит деньги на счет, второй не должен этого начинать.
            Console.WriteLine($"Изменение лицевого счета клиента {bankClient.FirstName} {bankClient.LastName}:");
            var clientAccount = clientService.GetClientAccounts(bankClient.ClientId).FirstOrDefault();
            if (clientAccount != null)
                Parallel.Invoke(
                    () =>
                    {
                        clientService.UpdateClientAccount(clientAccount.AccountId, amount: 45000);
                        Console.WriteLine(string.Join("Поток 1 - \n",
                            clientService.GetPresentationClientAccounts(bankClient.ClientId)));
                        clientService.Mutex.ReleaseMutex();
                    },
                    () =>
                    {
                        clientService.UpdateClientAccount(clientAccount.AccountId, amount: 32111);
                        Console.WriteLine(string.Join("Поток 2 - \n",
                            clientService.GetPresentationClientAccounts(bankClient.ClientId)));
                        clientService.Mutex.ReleaseMutex();
                    });

            // Изменение клиента.
            Console.WriteLine("Изменение клиента:");
            Parallel.Invoke(
                () =>
                {
                    clientService.UpdateClient(bankClient.ClientId, "Влад");
                    bankClient = clientService.ClientsWithFilterAndPagination(1, 1, clientId: bankClient.ClientId)
                        .FirstOrDefault();
                    if (bankClient != null)
                        Console.WriteLine(
                            $"Поток 1 - ID {bankClient.ClientId}, {bankClient.FirstName} {bankClient.LastName}, дата рождения {bankClient.DateOfBirth.ToString("D")}");
                    clientService.Mutex.ReleaseMutex();
                },
                () =>
                {
                    clientService.UpdateClient(bankClient.ClientId, lastName: "Юрченко");
                    bankClient = clientService.ClientsWithFilterAndPagination(1, 1, clientId: bankClient.ClientId)
                        .FirstOrDefault();
                    if (bankClient != null)
                        Console.WriteLine(
                            $"Поток 2 - ID {bankClient.ClientId}, {bankClient.FirstName} {bankClient.LastName}, дата рождения {bankClient.DateOfBirth.ToString("D")}");
                    clientService.Mutex.ReleaseMutex();
                });
        }
        bankingSystemDbContext.Dispose();
    }
}