using System;
using System.Collections.Generic;

namespace BankSystem.Models
{
	public class Bank : ITransfer<Account>
	{
		public List<Client> Clients { get; private set; }

		public Bank()
		{
			Clients = new List<Client>();
		}

		public void AddClient(string name)
		{
			Clients.Add(new Client(name));
		}

		public void OpenAccount(Client client, AccountType accountType)
		{
			client.OpenAccount(accountType);
		}

		public void CloseAccount(Account account)
		{
			account.Client.CloseAccount(account);
		}

		public void Transfer(Account fromAccount, Account toAccount, decimal amount)
		{
			if (fromAccount.Balance >= amount)
			{
				fromAccount.Balance -= amount;
				toAccount.Balance += amount;
			}
			else
			{
				throw new InvalidOperationException("Недостаточно средств.");
			}
		}
	}
}
