using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PayMeApp.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get; set;}
        public decimal Amount {get;set;}
        public DateTime CreatedAt {get;set;}

        public int  UserId {get; set;}
        public User GettingPayed {get; set;}
    }
}