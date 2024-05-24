using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppliComptaApi.src.Models
{
    public class AccountingDocumentDTO
    {
        public int Id { get; set; } 

        [Required]
        public string Number { get; set; }

        [Required]
     
        public DocumentType Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string FeeDesignation { get; set; }

        [Required]
        public int FeeAmount { get; set; }

        [Required]
        public int CommercialNet { get; set; }

        [Required]
        public int VAT { get; set; }

        [Required]
        public int NetPayable { get; set; }

        [Required]
        public int CustomerId { get; set; } 

    }

    

}
    

