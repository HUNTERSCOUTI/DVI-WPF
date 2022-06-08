using System;
using System.ComponentModel.DataAnnotations;

namespace DVI
{
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [Key]
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}