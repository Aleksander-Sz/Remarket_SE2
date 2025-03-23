using System;

namespace ReMarket.Models
{
//"The Payment class represents the financial component of the system. 
//It includes attributes such as ID, paidOn (a date), and total (a decimal value representing the payment amount). 
//Payments are directly linked to orders, ensuring that each order has an associated payment.

    public class Payment
    {
        public string ID { get; set; }
        public DateTime paidOn { get; set; }
        public decimal total { get; set; }

        public Payment(string ID, DateTime paidOn, decimal total)
        {
            this.ID=ID;
            this.paidOn=paidOn;
            this.total=total;
        }

    }
}