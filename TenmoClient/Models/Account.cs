using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Account
    {
        public int UserId { get; set; }

        public decimal Balance { get; set; } = 1000M;

    }
}
