using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

// originally should be placed into an Assembly.cs
[assembly: InternalsVisibleTo("Lufthansa.Logic.Tests")]

namespace Lufthansa.Data;

public class Brand : IDbItem, ICopyFrom<Brand>
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    /// <summary>
    /// Maximum distance in 1 flight
    /// </summary>
    [Required]
    public int? MaxFlightDistance { get; set; }

    public int NumberOfPassengerSeat { get; set; }

    [JsonIgnore]
    [NotMapped]
    public virtual ICollection<Airplane>? Airplanes { get; set; }

    public Brand CopyFrom(Brand other)
    {
        Id = other.Id;
        Name = other.Name;
        MaxFlightDistance = other.MaxFlightDistance;
        NumberOfPassengerSeat = other.NumberOfPassengerSeat;
        Airplanes = other.Airplanes?.ToList() ?? new List<Airplane>();
        return this;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        var other = obj as Brand;
        if (other == null)
        {
            return false;
        }

        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}