using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccountByUser(int userId);
        Account GetAccountById(int accountId);

        bool UpdateBalance(Transfer transfer);
    }
}
