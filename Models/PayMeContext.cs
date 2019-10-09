using Microsoft.EntityFrameworkCore;



namespace PayMeApp.Models
{
    public class PayMeContext : DbContext
    {
        public PayMeContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users {get;set;}
        public DbSet<Transaction> Transactions {get;set;}
    }


}
