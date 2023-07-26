using System.ComponentModel.DataAnnotations;
namespace RestfullWeb;

public partial class Catalog
{
    public int id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string name { get; set; } = null!;
    public int min_order { get; set; } 
    public int max_order { get; set; }
    public string measurement { get; set; } = null!;
    public int? company_id { get; set; }
}