using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Equivalence.Tests;

public class ClientGetHashCodeTests
{
    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualClients()
    {
        // Arrange
        var client = TestDataGenerator.GenerateRandomBankClient();
        var copiedClient = Client.CopyClient(client);

        // Act
        var hashCodeClient = client.GetHashCode();
        var hashCodeCopiedClient = copiedClient?.GetHashCode();

        // Assert
        Assert.Equal(hashCodeClient, hashCodeCopiedClient);
    }

    [Fact]
    public void GetHashCode_ReturnsDifferentValueForDifferentClients()
    {
        // Arrange
        var firstClient = TestDataGenerator.GenerateRandomBankClient();
        var secondClient = TestDataGenerator.GenerateRandomBankClient();
        
        // Act
        var hashCodeFirstClient = firstClient.GetHashCode();
        var hashCodeSecondClient = secondClient.GetHashCode();

        // Assert
        Assert.NotEqual(hashCodeFirstClient, hashCodeSecondClient);
    }
}