using System;

namespace dotnetapp.Models
{
    public class ExpenseTrackerApi
    {
        public int Id { get; set; }
        public DateTime Expense_Date { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
    }
}
