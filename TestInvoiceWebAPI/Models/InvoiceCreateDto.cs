using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TestInvoiceWebAPI.Models
{
    public class InvoiceCreateDto
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public List<InvoiceItemDto> Items { get; set; }
    }
}
