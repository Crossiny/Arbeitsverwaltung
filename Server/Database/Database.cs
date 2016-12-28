// Arbeitsverwaltung/Server/Database.cs
// by Christoph Schimpf, Jonathan Boeckel
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server.Database
{
    [Serializable]
    internal class Database : ISerializable
    {
        private Dictionary<string, string> _passwordDictionary = new Dictionary<string, string>();
        public Dictionary<string, User> UserDictionary = new Dictionary<string, User>();

        public Database()
        {
        }

        private Database(SerializationInfo info, StreamingContext context)
        {
            _passwordDictionary =
                info.GetValue("PasswordDictionary", typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            UserDictionary =
                info.GetValue("UserDictionary", typeof(Dictionary<string, User>)) as Dictionary<string, User>;
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PasswordDictionary", _passwordDictionary);
            info.AddValue("UserDictionary", UserDictionary);
        }

        #endregion

        public bool UserExists(string username)
        {
            return _passwordDictionary.ContainsKey(username) & UserDictionary.ContainsKey(username);
        }

        public bool CheckPassword(string username, string password)
        {
            if (!UserExists(username)) return false;
            return _passwordDictionary[username] == password;
        }

        public User GetUser(string username)
        {
            if (!UserExists(username)) return null;
            return UserDictionary[username];
        }

        public bool SetUser(string username, User user)
        {
            if (UserExists(username))
            {
                UserDictionary[username] = user;
                return true;
            }
            return false;
        }

        public bool AddUser(string username, string password)
        {
            if (UserExists(username))
                return false;
            _passwordDictionary.Add(username, password);
            UserDictionary.Add(username, new User(username));
            return true;
        }

        public bool GetIsAdmin(string username)
        {
            return UserExists(username) && UserDictionary[username].IsAdmin;
        }

        public void SetIsAdmin(string username, bool isAdmin)
        {
            if (UserExists(username))
            {
                UserDictionary[username].IsAdmin = isAdmin;
            }
        }

        public void AddShift(string username, Shift shift)
        {
            UserDictionary[username].AddShift(shift);
        }
    }
}