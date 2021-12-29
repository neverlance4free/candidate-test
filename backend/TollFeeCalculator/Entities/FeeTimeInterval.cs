using System;

namespace TollFeeCalculator.Entities
{
    public class FeeTimeInterval
    {
        public uint Fee { get; }
        public byte FromHour { get; }
        public byte FromMinutes { get; }
        public byte ToHour { get; }
        public byte ToMinutes { get; }

        public FeeTimeInterval(uint fee, byte fromHour, byte fromMin, byte toHour, byte toMin)
        {
            this.Fee = fee;
            this.FromHour = fromHour;
            this.FromMinutes = fromMin;
            this.ToHour = toHour;
            this.ToMinutes = toMin;
        }

        public bool IsInInterval(DateTime date)
        {
            return (date.Hour > this.FromHour || (date.Hour == this.FromHour && date.Minute >= this.FromMinutes))
                && (date.Hour < this.ToHour || (date.Hour == this.ToHour && date.Minute <= this.ToMinutes));
        }
    }
}
