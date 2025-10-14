using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class InvoiceItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemID { get; set; }

    [ForeignKey("Invoice")]
    public int InvoiceID { get; set; }

    public string Name { get; set; }
    public decimal Price { get; set; }

    [JsonIgnore]
    public Invoice Invoice { get; set; }
}
