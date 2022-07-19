using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lufthansa.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Lufthansa.WpfApp.ViewModel
{
    public class AirplaneVM : ObservableObject
    {
        public AirplaneVM()
        {

        }

        public AirplaneVM(Airplane airplane)
        {
            CopyFrom(airplane);
        }

        private int? _aggregatedFlownDistance;
        public int Id { get; set; }

        public virtual Brand Brand { get; set; }

        public int BrandId { get; set; }

        /// <summary>
        /// TODO: refactor to DateOnly
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// Distance flown in the whole life of the aircraft in km
        /// </summary>
        public int? AggregatedFlownDistance
        {
            get => _aggregatedFlownDistance;
            set
            {
                if (_aggregatedFlownDistance == value)
                {
                    return;
                }
                _aggregatedFlownDistance = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            var brand = this.Brand;
            return $"{nameof(Id)}:{Id}, {nameof(brand.Name)}:{brand.Name}, {nameof(ProductionDate)}:{ProductionDate:d}, {nameof(brand.MaxFlightDistance)}:{brand.MaxFlightDistance}, {nameof(AggregatedFlownDistance)}:{AggregatedFlownDistance}, ";
        }

        public void CopyFrom(Airplane other)
        {
            Id = other.Id;
            ProductionDate = other.ProductionDate;
            AggregatedFlownDistance = other.AggregatedFlownDistance;
            BrandId = other.BrandId;
            Brand = new Brand().CopyFrom(other.Brand);
        }

        public void CopyTo(Airplane other)
        {
            other.Id = Id;
            other.ProductionDate = ProductionDate;
            other.AggregatedFlownDistance = AggregatedFlownDistance;
            other.BrandId = BrandId;
            other.Brand = Brand;
        }

    }
}
