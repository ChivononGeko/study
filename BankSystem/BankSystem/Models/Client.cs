using System;
using System.Collections.Generic;
using System.Linq;

namespace BankSystem.Models
{
	public class Client
	{
		public string Name { get; private set; }
		public List<Account> Accounts { get; private set; }

		public Client(string name)
		{
			Name = name;
			Accounts = new List<Account>();
		}

		public void OpenAccount(AccountType accountType)
		{
			if (Accounts.Any(a => a.Type == accountType))
			{
				throw new InvalidOperationException($"” клиента уже есть учетна€ запись типа {accountType}.");
			}

			Accounts.Add(new Account(this, accountType));
		}

		public void CloseAccount(Account account)
		{
			Accounts.Remove(account);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
