using System;

namespace ReMarket.Models
{
    public struct Address
    {
    //The Address data type is utilized by the Order class to specify the shipping destination 
    //for purchased items. It ensures that orders are associated with precise delivery locations.
        public string street { get; }
        public string city { get; }
        public string state { get; }
        public string zipCode { get; }

        public Address(string _street, string _city, string _state, string _zipCode)
        {
            street = _street;
            city = _city;
            state = _state;
            zipCode = _zipCode;
        }
    }
}