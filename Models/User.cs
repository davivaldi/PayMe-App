using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayMeApp.Models
{
    public class User
    {
        [Key]
        public int UserId{get;set;}

        [Required]
        [Display(Name = "First Name")]
        [MinLength(3, ErrorMessage = "Must be at least 2 characters long")]
        public string FirstName {get; set;}
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(3, ErrorMessage = "Must be at least 2 characters long")]
        public string LastName {get; set;}
        [Required]
        [EmailAddress]
        [MinLength(3, ErrorMessage = "Must be at least 2 characters long")]
        public string Email {get; set;}

        public decimal Balance {get;set;}

        [Required]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [NotMapped]
        public List<decimal> UsersTransactions {get;set;}



    }
}