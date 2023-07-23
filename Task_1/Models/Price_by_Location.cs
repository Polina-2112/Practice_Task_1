using System.ComponentModel.DataAnnotations;
namespace RestfullWeb;
public partial class Price_by_Location
{
    public int Id { get; set; }
    public int Price { get; set; }
    public bool Price_on_request { get; set; }
    public int? LocationId { get; set; }
    public int? CatalogId { get; set; }
}