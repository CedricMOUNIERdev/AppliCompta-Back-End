using System.ComponentModel.DataAnnotations;

namespace AppliComptaApi.src.Models
{
    public class Customer
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [MaxLength(7, ErrorMessage = "Number must be less than 7 characters long.")]
        public string Number { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MaxLength(5, ErrorMessage = "PostalCode must be less than 5 characters long.")]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }


        [MaxLength(10, ErrorMessage ="TelephoneNumber must be less than 10 characters long.")]
        public string? TelephoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        // One-to-many relationship with AccountingDocument

        public ICollection<AccountingDocument>? AccountingDocuments { get; set; } 
    }
}
