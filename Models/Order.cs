using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace BooksSpring2024_sec02.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public string CustomerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string PostalCode { get; set; }
        public string Phone {  get; set; }
        public decimal OrderTotal { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentStatus { get; set; }

        public DateOnly OrderDate { get; set; }

        public string? Carrier {  get; set; }

        public string? TrackingNumber { get; set; }

        public DateOnly? ShippingDate { get; set; }

        public string? SessionID { get; set; }

        public string? PaymentIntentID { get; set; }

    }
}
