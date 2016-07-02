using System;
using System.Collections.Generic;

namespace Server.Database
{
    class User
    {
        public string Username { get; }
        public int Loan { get; }
        public List<Shift> Shifts = new List<Shift>();

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

        public User(string username, int loan)
        {
            Username = username;
            Loan = loan;
        }
    }
}