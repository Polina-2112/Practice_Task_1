using System.ComponentModel.DataAnnotations;
namespace RestfullWeb;
public partial class Price_by_Location
{
    public int id { get; set; }
    public int price { get; set; }
    public bool price_on_request { get; set; }
    public int? location_id { get; set; }
    public int? catalog_id { get; set; }
}