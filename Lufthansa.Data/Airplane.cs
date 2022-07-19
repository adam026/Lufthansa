using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lufthansa.Data;

public class Airplane : IDbItem, ICopyFrom<Airplane>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [NotMapped]
    public virtual Brand Brand { get; set; }

    public int BrandId { get; set; }

    /// <summary>
    /// TODO: refactor to DateOnly
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Distance flown in the whole life of the aircraft in km
    /// </summary>
    public int? AggregatedFlownDistance { get; set; }

    public override string ToString()
    {
        var brand = this.Brand;
        return $"{nameof(Id)}:{Id}, {nameof(brand.Name)}:{brand.Name}, {nameof(ProductionDate)}:{ProductionDate:d}, {nameof(brand.MaxFlightDistance)}:{brand.MaxFlightDistance}, {nameof(AggregatedFlownDistance)}:{AggregatedFlownDistance}, ";
    }

    public Airplane CopyFrom(Airplane other)
    {
        Id = other.Id;
        ProductionDate = other.ProductionDate;
        AggregatedFlownDistance = other.AggregatedFlownDistance;
        BrandId = other.BrandId;
        Brand = new Brand().CopyFrom(other.Brand);
        return this;
    }

}

