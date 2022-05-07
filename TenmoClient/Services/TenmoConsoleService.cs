using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        // Add application-specific UI methods here...

        public void GetBalance(TenmoApiService apiService)
        {
            Account userAccount = apiService.GetAccount(apiService.UserId);

            Console.WriteLine($"Your account balance is ${userAccount.Balance}");
        }

        public List<ApiUser> GetUsers(TenmoApiService apiService)
        {
            List<ApiUser> users = apiService.GetUsers();

            Console.WriteLine("|------------ Users ------------|");
            foreach(ApiUser u in users)
            {
                Console.WriteLine($"{u.UserId} | {u.Username}");
            }
            Console.WriteLine("|-------------------------------|");

            return users;
        }

        public List<Transfer> GetOwnTransfers(TenmoApiService apiService)
        {
            List<Transfer> transfers = apiService.GetOwnTransfers(apiService.UserId);
            List<ApiUser> users = apiService.GetUsers();

            Console.WriteLine("|------------ Your Transfers ------------|");
            foreach (Transfer t in transfers)
            {
                string username = "";
                foreach (ApiUser user in users)
                {
                    if (t.AccountTo == user.UserId)
                    {
                        username = user.Username;
                    }
                }
                string fromTo = $"To: {username}";
                Console.WriteLine($"{t.TransferId} | {fromTo} | ${t.Amount}");
            }
            Console.WriteLine("|-------------------------------|");

            return transfers;
        }

        public void GetTransferDetails(TenmoApiService apiService, int transferId)
        {
            Transfer transfer = apiService.GetTransfer(transferId);
            List<ApiUser> users = apiService.GetUsers();
            string fromName = "";
            string toName = "";

            foreach (ApiUser user in users)
            {
                if (transfer.AccountFrom == user.UserId)
                {
                    fromName = user.Username;
                }
                else if (transfer.AccountTo == user.UserId)
                {
                    toName = user.Username;
                }

            }       

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Id: {transfer.TransferId}");
            Console.WriteLine($"From: {fromName}");
            Console.WriteLine($"To: {toName}");
            Console.WriteLine($"Type: {apiService.GetType(transfer.TransferTypeId).TypeDescription}");
            Console.WriteLine($"Status: {apiService.GetStatus(transfer.TransferStatusId).StatusDescription}");
            Console.WriteLine($"Amount: ${transfer.Amount}");           
        }
    }
}
