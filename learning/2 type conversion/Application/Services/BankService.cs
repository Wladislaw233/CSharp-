﻿using System.Collections.Generic;
using Models;
namespace Services
{
    public class BankService
    {
        public void CalculateSalaryOfBankOwners(double profit, double expenses, List<Employee> owners)
        {
            double salary = (profit - expenses) / owners.Count;
            foreach (var owner in owners) owner.Salary = (int)salary;
        }
    }
}