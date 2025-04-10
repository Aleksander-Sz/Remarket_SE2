using System;

namespace ReMarket.Models
{
//"The Email data type ensures standardization and validation of email addresses 
//for both user accounts and communications within the platform."
    public struct Email
    {
        public string Value { get; }

        public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains("@") || !value.Contains("."))
            throw new ArgumentException("Invalid email format");
        
        Value = value;
    }

    public override string ToString() => Value;
    }
}
