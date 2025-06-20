using System;

namespace ReMarket.Models
{
//"The Payment class represents the financial component of the system. 
//It includes attributes such as ID, paidOn (a date), and total (a decimal value representing the payment amount). 
//Payments are directly linked to orders, ensuring that each order has an associated payment.

    public class Payment
    {
        public int Id { get; set; }
        public DateTime PaidOn { get; set; }
        public decimal Total { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public Payment() { }

        public Payment(int ID, DateTime paidOn, decimal total)
        {
            this.Id=ID;
            this.PaidOn=paidOn;
            this.Total=total;
        }

    }
    public record PaymentRequest(
        int OrderId,
        string CardNumber,
        string CardCVC,
        string CardExpirationMonth,
        string CardExpirationYear);
}