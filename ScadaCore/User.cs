using System.ComponentModel.DataAnnotations;

namespace ScadaCore
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        public string EncryptedPassword { get; set; }

        public User() { }

        public User(string username, string encryptedPassword)
        {
            Username = username;
            EncryptedPassword = encryptedPassword;
        }
    }
}
