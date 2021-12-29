using Nager.Date;
using System;
using System.Collections.Generic;
using TollFeeCalculator.Entities;

namespace TollFeeCalculator.BLL
{
    public abstract class BaseTollCalculator
    {
        public CountryCode Country { get; protected set; }
        public uint? MaxFeePerDay { get; protected set; }

        public abstract uint GetFee(IVehicle vehicle, IEnumerable<DateTime> dates);
        public abstract bool IsTollFree(IVehicle vehicle);
        public abstract bool IsTollFree(DateTime date);
        public abstract uint GetFeeForTime(DateTime date);
    }
}