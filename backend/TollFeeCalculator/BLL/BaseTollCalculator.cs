using Nager.Date;
using System;
using System.Collections.Generic;
using TollFeeCalculator.Entities;

namespace TollFeeCalculator.BLL
{
    public interface ITollCalculator
    {
        public uint GetFee(IVehicle vehicle, IEnumerable<DateTime> dates);
    }

    public abstract class BaseTollCalculator : ITollCalculator
    {
        public CountryCode Country { get; protected set; }

        public uint? MaxFeePerDay { get; protected set; }

        public abstract uint GetFee(IVehicle vehicle, IEnumerable<DateTime> dates);

        public virtual bool IsTollFree(IVehicle vehicle) => false;

        public virtual bool IsTollFree(DateTime date) => DateSystem.IsWeekend(date, this.Country) || DateSystem.IsPublicHoliday(date, this.Country);
    }
}