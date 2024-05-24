using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppliComptaApi.src.Models
{
    public class AccountingDocument
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [MaxLength(8, ErrorMessage = "Number must be less than 8 characters long.")]
        public string Number { get; set; }

        [Required]
        [EnumDataType(typeof(DocumentType))]
        public DocumentType Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string FeeDesignation { get; set; }

        [Required]
        public int FeeAmount  { get; set; }

        [Required]
        public int CommercialNet { get; set; }

        [Required]
        public int VAT { get; set; }

        [Required]
        public int NetPayable { get; set; }

        //One-to-many relationship with Customer
        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; } // Foreign key property

        public Customer Customer { get; set; } // Navigation property to represent the customer

    }

    public enum DocumentType: int
    {
        Invoice = 0,
        Credit = 1,
    }

}
