using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel;
using Microsoft.EntityFrameworkCore;

namespace ScadaCore
{
    public class Authentication : IAuthentication, ISimpleService
    {
        private readonly UsersContext _context;

        public Authentication()
        {
            _context = new UsersContext();
        }

        public bool Registration(string username, string password)
        {
            try
            {
                string encryptedPassword = Security.EncryptData(password);
                User user = new User(username, encryptedPassword);

                _context.Users.Add(user);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex.Message)
                return false;
            }
        }

        public string Login(string username, string password)
        {
            try
            {
                foreach (var user in _context.Users)
                {
                    if (username == user.Username && Security.ValidateEncryptedData(password, user.EncryptedPassword))
                    {
                        string token = GenerateToken(username);
                        authenticatedUsers.Add(token, user);
                        return token;
                    }
                }

                return "Login failed";
            }
            catch (Exception ex)
            {
                // Log the exception (ex.Message)
                return "Internal error";
            }
        }

        public bool Logout(string token)
        {
            return authenticatedUsers.Remove(token);
        }

        private string GenerateToken(string username)
        {
            using (var crypto = new RNGCryptoServiceProvider())
            {
                byte[] randVal = new byte[32];
                crypto.GetBytes(randVal);
                string randStr = Convert.ToBase64String(randVal);
                return username + randStr;
            }
        }

        private bool IsUserAuthenticated(string token)
        {
            return authenticatedUsers.ContainsKey(token);
        }

        public string GetSomeMessage(string token)
        {
            if (IsUserAuthenticated(token))
                return "Hello authenticated user!";
            else
                return "You have to login first!";
        }

        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();
    }
}
