using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using Microsoft.EntityFrameworkCore;

namespace ScadaCore
{
    public class Authentication : IAuthentication, ISimpleService
    {
        private readonly UsersContext _context;
        private static List<Alarm> alarms = new List<Alarm>();
        private const string AlarmsLogFilePath = "alarmsLog.txt";

        public Authentication()
        {
            _context = new UsersContext();
        }

        public void AddAlarm(string tagName, string type, int priority, double threshold)
        {
            var alarm = new Alarm(tagName, type, priority, threshold);
            alarms.Add(alarm);
            LogAlarm(alarm);
        }

        public void RemoveAlarm(string tagName)
        {
            var alarmToRemove = alarms.FirstOrDefault(a => a.TagName == tagName);
            if (alarmToRemove != null)
            {
                alarms.Remove(alarmToRemove);
            }
        }

        public List<Alarm> GetActiveAlarms()
        {
            return alarms;
        }

        private void LogAlarm(Alarm alarm)
        {
            using (StreamWriter sw = File.AppendText(AlarmsLogFilePath))
            {
                sw.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated At: {alarm.ActivationTime}");
            }
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
