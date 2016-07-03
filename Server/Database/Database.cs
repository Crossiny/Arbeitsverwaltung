using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server.Database
{
    [Serializable]
    internal class Database : ISerializable
    {
        private readonly Dictionary<string, string> _passwordDictionary = new Dictionary<string, string>();
        private readonly Dictionary<string, User> _userDictionary = new Dictionary<string, User>();

        public Database()
        {
        }

        private Database(SerializationInfo info, StreamingContext context)
        {
            _passwordDictionary =
                info.GetValue("PasswordDictionary", typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            _userDictionary =
                info.GetValue("UserDictionary", typeof(Dictionary<string, User>)) as Dictionary<string, User>;
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PasswordDictionary", _passwordDictionary);
            info.AddValue("UserDictionary", _userDictionary);
        }

        #endregion

        public bool UserExists(string username)
        {
            return _passwordDictionary.ContainsKey(username) & _userDictionary.ContainsKey(username);
        }

        public bool CheckPassword(string username, string password)
        {
            if (!UserExists(username)) return false;
            return _passwordDictionary[username] == password;
        }

        public User GetUser(string username)
        {
            if (!UserExists(username)) return null;
            return _userDictionary[username];
        }

        public bool SetUser(string username, User user)
        {
            if (UserExists(username))
            {
                _userDictionary[username] = user;
                return true;
            }
            return false;
        }

        public bool AddUser(string username, string password)
        {
            if (UserExists(username))
                return false;
            _passwordDictionary.Add(username, password);
            _userDictionary.Add(username, new User(username));
            return true;
        }

        public bool IsAdmin(string username)
        {
            return UserExists(username) && _userDictionary[username].IsAdmin;
        }

        public void AddShift(string username, Shift shift)
        {
            _userDictionary[username].AddShift(shift);
        }
    }
}