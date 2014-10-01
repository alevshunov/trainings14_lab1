using System;
using ImhoNet.Domain.Model;

namespace ImhoNet.Domain.Helper
{
    internal class PotentialUser
    {
        public PotentialUser(User user, double correlationValue)
        {
            User = user;
            CorrelationValue = correlationValue;
        }

        public User User { get; private set; }

        public double CorrelationValue { get; private set; }

        public int CompareTo(PotentialUser other)
        {
            return CorrelationValue.CompareTo(other.CorrelationValue);
        }
    }
}