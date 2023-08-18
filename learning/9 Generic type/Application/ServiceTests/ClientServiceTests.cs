using Models;
using Services;
using Services.Exceptions;
using Services.Storage;

namespace ServiceTests;

public class ClientServiceTests
{
    private readonly ClientStorage _clientStorage = new();
    private List<Client> _bankClients = new();

    public void AddClientTest()
    {
        _bankClients = TestDataGenerator.GenerateListWitchBankClients(2, true);

        // Клиент без возвраста.
        _bankClients[1].Age = 0;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Тесты сервисов для клиентов и сотрудников.");
        Console.WriteLine("КЛИЕНТЫ");
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добавление клиентов:");
            Console.ResetColor();
            foreach (var client in _bankClients)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {client.FirstName}, фамилия: {client.LastName}, возраст: {client.Age}");
                _clientStorage.Add(client);
                ClientService.WithdrawClientAccounts(client, _clientStorage.Data[client]);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Произошла ошибка при добавлении клиента: ", exception);
        }
    }

    public void AddClientAccountTest()
    {
        _clientStorage.WithdrawBankCurrencies();
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nДобавление счета клиентам:");
            Console.ResetColor();
            Console.WriteLine("Добавим счет EUR:");
            _clientStorage.AddAccount(_bankClients[0], "EUR", 13567.12);
            ClientService.WithdrawClientAccounts(_bankClients[0], _clientStorage.Data[_bankClients[0]]);
            Console.WriteLine("Добавим счет RUP (такой валюты нет):");
            _clientStorage.AddAccount(_bankClients[1], "RUP", 123.1);
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Произошла ошибка при добавлении счета клиенту: ", exception);
        }
    }

    public void UpdateClientAccountTest()
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nРедактирование счета клиента:");
            Console.ResetColor();
            var clientAccounts = ClientService.GetClientAccounts(_clientStorage.Data, _bankClients[0]);
            Console.WriteLine("Изменим счет EUR на счет RUB:\nДо изменения:");
            ClientService.WithdrawClientAccounts(_bankClients[0], clientAccounts);
            _clientStorage.UpdateAccount(_bankClients[0], clientAccounts[1].AccountNumber, "RUB", 45677.23);
            Console.WriteLine("\nПосле изменения:");
            ClientService.WithdrawClientAccounts(_bankClients[0], clientAccounts);
            Console.WriteLine("Изменим другому клиенту счет EUR(у него его нет) на счет RUB:\nДо изменения:");
            clientAccounts = ClientService.GetClientAccounts(_clientStorage.Data, _bankClients[1]);
            ClientService.WithdrawClientAccounts(_bankClients[1], clientAccounts);
            _clientStorage.UpdateAccount(_bankClients[1], clientAccounts[0].AccountNumber.Replace("USD", "EUR"),
                "RUB",45677.23);
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Произошла ошибка при редактировании счета клиента: ", exception);
        }
    }
}