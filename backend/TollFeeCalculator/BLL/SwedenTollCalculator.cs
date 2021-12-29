using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator.Entities;

namespace TollFeeCalculator.BLL
{
    public class SwedenTollCalculator : BaseTollCalculator
    {
        public SwedenTollCalculator()
        {
            this.Country = CountryCode.SE;
            this.MaxFeePerDay = 60;
        }

        public override uint GetFee(IVehicle vehicle, IEnumerable<DateTime> dates)
        {
            if (vehicle is null) throw new ArgumentNullException(nameof(vehicle));
            if (dates is null) throw new ArgumentNullException(nameof(dates));

            if (!dates.Any()) return 0;
            if (this.IsTollFree(vehicle) || this.IsTollFree(dates.First())) return 0;

            uint totalFee = 0;
            DateTime? lastFeeDate = null;

            foreach (var date in dates)
            {
                if (this.MaxFeePerDay.HasValue && totalFee >= this.MaxFeePerDay)
                {
                    totalFee = this.MaxFeePerDay.Value;
                    break;
                }

                if(lastFeeDate.HasValue && date.TimeOfDay.Subtract(lastFeeDate.Value.TimeOfDay).Hours == 0)
                {
                    break;
                }

                totalFee += this.GetFeeForTime(date);
                lastFeeDate = date;
            }
            return totalFee;
        }

        public override bool IsTollFree(IVehicle vehicle)
        {
            return vehicle.Type != VehicleType.Car;
        }

        public override bool IsTollFree(DateTime date)
        {
            return DateSystem.IsWeekend(date, this.Country) || DateSystem.IsPublicHoliday(date, this.Country);
        }

        public override uint GetFeeForTime(DateTime date)
        {
            return _feePeriods.FirstOrDefault(x => x.IsInInterval(date))?.Fee ?? 0;
        }

        private readonly List<FeeTimeInterval> _feePeriods = new List<FeeTimeInterval>
        {
            new FeeTimeInterval(0, 0, 0, 5, 59),
            new FeeTimeInterval(9, 6, 0, 6, 29),
            new FeeTimeInterval(16, 6, 30, 6, 59),
            new FeeTimeInterval(22, 7, 00, 7, 59),
            new FeeTimeInterval(16, 8, 0, 8, 29),
            new FeeTimeInterval(9, 8, 30, 14, 59),
            new FeeTimeInterval(16, 15, 0, 15, 29),
            new FeeTimeInterval(22, 15, 30, 16, 59),
            new FeeTimeInterval(16, 17, 0, 17, 59),
            new FeeTimeInterval(9, 18, 0, 18, 29),
            new FeeTimeInterval(0, 18, 30, 0, 0)
        };
    }
}