using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {               
                // View your current balance
                console.GetBalance(tenmoApiService);
                console.Pause();
                
            }

            if (menuSelection == 2)
            {
                // View your past transfers
                //console.GetOwnTransfers(tenmoApiService);
                //int transferId = console.PromptForInteger("Please enter transfer id to view details(0)");
                //console.GetTransferDetails(tenmoApiService, transferId);
                //console.Pause();
                ViewTransfers();
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                SendMoney();
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        public void SendMoney()
        {
            List<ApiUser> users = console.GetUsers(tenmoApiService);
            bool validUser = false;
            int accountTo = console.PromptForInteger("Id of the user you are sending money to(0)");

            if (accountTo == 0)
            {
                return;
            }

            foreach (ApiUser user in users)
            {
                if (accountTo == user.UserId)
                {
                    validUser = true;
                }
            }

            if (!validUser)
            {
                console.PrintError("Invalid user Id.");
                console.Pause();
                return;
            }
            if (accountTo == tenmoApiService.UserId)
            {
                console.PrintError("You cannot send money to yourself.");
                console.Pause();
                return;
            }            
            decimal amount = console.PromptForDecimal("Enter amount to send");

            if (amount <= 0)
            {
                console.PrintError("Amount to send must be greater than 0.");
                console.Pause();
                return;
            }
            Account userAccount = tenmoApiService.GetAccount(tenmoApiService.UserId);
            if (userAccount.Balance < amount)
            {
                console.PrintError("Not enough money in balance.");
                console.Pause();
                return;
            }
            Transfer transfer = new Transfer();
            transfer.TransferStatusId = 2;
            transfer.TransferTypeId = 2;
            transfer.AccountFrom = tenmoApiService.UserId;
            transfer.AccountTo = accountTo;
            transfer.Amount = amount;
            
            tenmoApiService.AddTransfer(transfer);
        }

        public void ViewTransfers()
        {           
            List<Transfer> transfers = console.GetOwnTransfers(tenmoApiService);
            List<int> transferIds = new List<int>();
            foreach (Transfer t in transfers)
            {
                transferIds.Add(t.TransferId);
            }

            int transferId = console.PromptForInteger("Please enter transfer id to view details(0)");

            if (transferId == 0)
            {
                return;
            }
            else if (!transferIds.Contains(transferId))
            {
                console.PrintError("Invalid transfer Id. Choose from your list of transfers.");
                console.Pause();
                return;
            }
            console.GetTransferDetails(tenmoApiService, transferId);
            console.Pause();
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }
    }
}
