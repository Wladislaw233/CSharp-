using Models;
using Services;
using Services.Exceptions;

namespace PracticeWithException;

internal class Program
{
    public static void Main(string[] args)
    {
        // КЛИЕНТЫ
        var clientService = new ClientService();
        var clients = TestDataGenerator.GenerateListWitchBankClients(2, true);

        // Клиент без возвраста.
        clients[1].Age = 0;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("КЛИЕНТЫ");
        // Добавление клиента.
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добавление клиентов:");
            Console.ResetColor();
            foreach (var client in clients)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {client.FirstName}, фамилия: {client.LastName}, возраст: {client.Age}");
                var clientAccounts = clientService.AddClient(client);
                WithdrawClientAccounts(client, clientAccounts);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            ExceptionHandling("Произошла ошибка при добавлении клиента: ", exception);
        }

        clientService.WithdrawBankCurrencies();

        // Добавление счета клиенту.
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nДобавление счета клиентам:");
            Console.ResetColor();
            Console.WriteLine("Добавим счет EUR:");
            var clientAccounts = clientService.AddClientAccount(clients[0], "EUR", 13567.12);
            WithdrawClientAccounts(clients[0], clientAccounts);
            Console.WriteLine("Добавим счет RUP (такой валюты нет):");
            clientService.AddClientAccount(clients[1], "RUP", 123.1);
        }
        catch (CustomException exception)
        {
            ExceptionHandling("Произошла ошибка при добавлении счета клиенту: ", exception);
        }

        // Редактирование счета клиента.
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nРедактирование счета клиента:");
            Console.ResetColor();
            var clientAccounts = clientService.GetClientAccounts(clients[0]);
            Console.WriteLine("Изменим счет EUR на счет RUB:\nДо изменения:");
            WithdrawClientAccounts(clients[0], clientAccounts);
            clientService.UpdateClientAccount(clients[0], clientAccounts[1].AccountNumber, "RUB", 45677.23);
            Console.WriteLine("\nПосле изменения:");
            WithdrawClientAccounts(clients[0], clientAccounts);
            Console.WriteLine("Изменим другому клиенту счет EUR(у него его нет) на счет RUB:\nДо изменения:");
            clientAccounts = clientService.GetClientAccounts(clients[1]);
            WithdrawClientAccounts(clients[1], clientAccounts);
            clientService.UpdateClientAccount(clients[1], clientAccounts[0].AccountNumber.Replace("USD", "EUR"), "RUB",
                45677.23);
        }
        catch (CustomException exception)
        {
            ExceptionHandling("Произошла ошибка при редактировании счета клиента: ", exception);
        }

        // СОТРУДНИКИ
        var employeeService = new EmployeeService();
        var employees = TestDataGenerator.GenerateListWithEmployees(2, true);
        employees[1].Contract = "";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("СОТРУДНИКИ");
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добавление сотрудников:");
            Console.ResetColor();
            foreach (var employee in employees)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}");
                employeeService.AddBankEmployee(employee);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            ExceptionHandling("При добавлении сотрудника произошла ошибка: ", exception);
        }

        Console.WriteLine("\nСписок сотрудников после добавления:\n" + string.Join('\n',
            employeeService.BankEmployees.Select(employee =>
                $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}")));
    }

    private static void ExceptionHandling(string description, CustomException exception)
    {
        Console.WriteLine(description + exception.Message + (string.IsNullOrWhiteSpace(exception.ParameterOfException)
            ? ""
            : $"\nПараметр: {exception.ParameterOfException}"));
    }

    private static void WithdrawClientAccounts(Client client, List<Account> clientAccounts)
    {
        Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, лицевые счета:" +
                          $"\n" + string.Join('\n',
                              clientAccounts.Select(clientAccount =>
                                  $"Номер счета: {clientAccount.AccountNumber} валюта: {clientAccount.Currency.Name} " +
                                  $"баланс: {clientAccount.Amount} {clientAccount.Currency.Code}")) +
                          "\n");
    }
}