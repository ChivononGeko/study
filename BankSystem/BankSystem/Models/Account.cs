namespace BankSystem.Models
{
	public class Account
	{
		public Client Client { get; private set; }
		public decimal Balance { get; set; }
		public AccountType Type { get; private set; }

		public Account(Client client, AccountType type)
		{
			Client = client;
			Type = type;
			Balance = 0;
		}

		public void Deposit(decimal amount)
		{
			Balance += amount;
		}

		public override string ToString()
		{
			return $"{Type} —чет: {Balance:C}";
		}
	}
}
