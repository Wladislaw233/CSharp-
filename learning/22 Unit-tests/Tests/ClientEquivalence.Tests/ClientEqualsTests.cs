﻿using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Equivalence.Tests;

public class ClientEqualsTests
{
    private readonly TestDataGenerator _testDataGenerator = new();
    
    [Fact]
    public void Equals_ReturnsTrueForIdenticalClients()
    {
        // Arrange
        var client = _testDataGenerator.GenerateRandomBankClient();
        var copiedClient = Client.CopyClient(client);
        
        // Act
        var isIdentical = client.Equals(copiedClient);
        
        // Assert
        Assert.True(isIdentical, "equals should return true for identical clients.");
    }

    [Fact]
    public void Equals_ReturnFalseForDifferentClients()
    {
        // Arrange
        var firstClient = _testDataGenerator.GenerateRandomBankClient();
        var secondClient = _testDataGenerator.GenerateRandomBankClient();
        
        // Act
        var isIdentical = firstClient.Equals(secondClient);
        
        // Assert
        Assert.False(isIdentical, "equals should return false for different clients.");
    }
}