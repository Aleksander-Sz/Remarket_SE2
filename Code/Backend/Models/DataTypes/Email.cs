using System;

namespace ReMarket.Models
{
//"The Email data type ensures standardization and validation of email addresses 
//for both user accounts and communications within the platform."
    public struct Email
    {
        public string value { get; }

        public Email(string _value)
        {
            if (!_value.Contains("@"))
                throw new ArgumentException("Invalid email format");
            
            value = _value;
        }
    }
}