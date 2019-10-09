using System.Collections.Generic;




namespace PayMeApp.Models
{
    public class DashboardViewModel
    {
        public User User {get;set;}
        public Transaction Transaction {get;set;}
        public List<Transaction> Transactions {get;set;}
    }
}