using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagmentSystem.Models
{
    public enum UserRole
    {
        Manager,
        Client,
        Employee
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string UserPassword { get; set; }

        [Required]
        public UserRole UserRole { get; set; }
        
        // Constructor to auto-generate username and password
       public User() {}

        public string GenerateUserName(string firstName, string lastName)
        {
            return $"{firstName.ToLower()}.{lastName.ToLower()}.{new Random().Next(100, 999)}";
        }

        public string GeneratePassword(int length = 8)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random random = new Random();
            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(password);
        }
    }
}


