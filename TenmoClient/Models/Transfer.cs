using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferStatusId { get; set; }
        public int TransferTypeId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransferType
    {
        public int TransferId { get; set; }
        public string TypeDescription { get; set; }
    }

    public class TransferStatus
    {
        public int TransferStatusId { get; set; }
        public string StatusDescription { get; set; }
    }
}
