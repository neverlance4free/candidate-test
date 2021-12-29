using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Entities
{
    public interface IVehicle
    {
        VehicleType Type { get; }
    }

    public class Vehicle : IVehicle
    {
        public VehicleType Type { get; }

        public Vehicle(VehicleType type) => Type = type;
    }

    public enum VehicleType
    {
        Motorbike,
        Car,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
}