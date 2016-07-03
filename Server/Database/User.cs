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

        public string Username;
        public double Loan;
        public bool IsAdmin;

        public TimeSpan WorkSpan
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

        public TimeSpan WorkSpanRange(DateTime startTime, DateTime endTime)
        {
            TimeSpan timeSpan = default(TimeSpan);
            foreach (Shift shift in Shifts)
            {
                if (shift.StartTime > startTime && shift.EndTime < endTime)
                    timeSpan += shift.WorkSpan;
            }
            return timeSpan;
        }

        public TimeSpan BreakedSpanRange(DateTime startTime, DateTime endTime)
        {
            TimeSpan timeSpan = default(TimeSpan);
            foreach (Shift shift in Shifts)
            {
                if (shift.StartTime > startTime && shift.EndTime < endTime)
                    timeSpan += shift.BreakSpan;
            }
            return timeSpan;
        }
        
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Loan", Loan);
            info.AddValue("Shifts", Shifts);
        }
        
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