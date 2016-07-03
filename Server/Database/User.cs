using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server.Database
{
    [Serializable]
    public class User : ISerializable
    {
        public List<Shift> Shifts = new List<Shift>();

        public User(string username)
        {
            Username = username;
        }

        private User(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            Loan = info.GetDouble("Loan");
            Shifts = info.GetValue("Shifts", typeof(List<Shift>)) as List<Shift>;
        }

        public string Username { get; }
        public double Loan { get; private set; }
        public bool IsAdmin { get; private set; }

        public TimeSpan WorkedSpan
        {
            get
            {
                TimeSpan span = default(TimeSpan);
                foreach (Shift shift in Shifts)
                {
                    span += shift.WorkSpan;
                }
                return span;
            }
        }

        public TimeSpan BreakSpan
        {
            get
            {
                TimeSpan breakSpan = default(TimeSpan);
                foreach (Shift shift in Shifts)
                {
                    breakSpan += shift.BreakSpan;
                }
                return breakSpan;
            }
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Loan", Loan);
            info.AddValue("Shifts", Shifts);
        }

        #endregion

        public void SetLoan(double loan)
        {
            Loan = loan;
        }

        public void SetAdmin(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public void AddShift(Shift shift)
        {
            Shifts.Add(shift);
        }
    }
}