namespace BankSystem.Models
{
    public interface ITransfer<in T>
    {
        void Transfer(T fromAccount, T toAccount, decimal amount);
    }
}
