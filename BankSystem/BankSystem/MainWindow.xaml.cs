using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BankSystem.Models;

namespace BankSystem
{
    public partial class MainWindow : Window
    {
        private Bank _bank;

        public MainWindow()
        {
            InitializeComponent();
            _bank = new Bank();
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            var clientName = txtClientName.Text;
            if (clientName == "Client Name" || string.IsNullOrWhiteSpace(clientName))
            {
                MessageBox.Show("Please enter a valid client name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _bank.AddClient(clientName);
            UpdateClientList();
            txtClientName.Text = "Client Name";
            txtClientName.Foreground = Brushes.Gray;
        }

        private void btnOpenAccount_Click(object sender, RoutedEventArgs e)
        {
            if (lstClients.SelectedItem == null) return;
            var client = (Client)lstClients.SelectedItem;
            var accountType = cmbAccountType.SelectedIndex == 0 ? AccountType.NonDeposit : AccountType.Deposit;

            if (client.Accounts.Any(a => a.Type == accountType))
            {
                MessageBox.Show($"Client already has an account of type {accountType}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _bank.OpenAccount(client, accountType);
            UpdateAccountList(client);
        }

        private void btnCloseAccount_Click(object sender, RoutedEventArgs e)
        {
            if (lstAccounts.SelectedItem == null) return;
            var account = (Account)lstAccounts.SelectedItem;
            _bank.CloseAccount(account);
            UpdateAccountList((Client)lstClients.SelectedItem);
        }

        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (lstAccounts.SelectedItems.Count != 2)
            {
                MessageBox.Show("Please select exactly two accounts for the transfer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var fromAccount = (Account)lstAccounts.SelectedItems[0];
            var toAccount = (Account)lstAccounts.SelectedItems[1];

            if (fromAccount.Client != toAccount.Client)
            {
                MessageBox.Show("Transfers are only allowed between accounts of the same client.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtTransferAmount.Text, out var amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _bank.Transfer(fromAccount, toAccount, amount);
                UpdateAccountList((Client)lstClients.SelectedItem);
                txtTransferAmount.Text = "Transfer Amount";
                txtTransferAmount.Foreground = Brushes.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            if (lstAccounts.SelectedItem == null) return;
            var account = (Account)lstAccounts.SelectedItem;

            if (!decimal.TryParse(txtDepositAmount.Text, out var amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            account.Deposit(amount);
            UpdateAccountList((Client)lstClients.SelectedItem);
            txtDepositAmount.Text = "Deposit Amount";
            txtDepositAmount.Foreground = Brushes.Gray;
        }

        private void lstClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstClients.SelectedItem != null)
                UpdateAccountList((Client)lstClients.SelectedItem);
        }

        private void UpdateClientList()
        {
            lstClients.ItemsSource = null;
            lstClients.ItemsSource = _bank.Clients;
        }

        private void UpdateAccountList(Client client)
        {
            lstAccounts.ItemsSource = null;
            lstAccounts.ItemsSource = client.Accounts;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                if (textBox == txtClientName) textBox.Text = "Client Name";
                else if (textBox == txtTransferAmount) textBox.Text = "Transfer Amount";
                else if (textBox == txtDepositAmount) textBox.Text = "Deposit Amount";
            }
        }
    }
}
