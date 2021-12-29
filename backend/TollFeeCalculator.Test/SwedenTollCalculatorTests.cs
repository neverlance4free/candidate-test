using System;
using System.Collections.Generic;
using TollFeeCalculator.BLL;
using TollFeeCalculator.Entities;
using Xunit;

namespace TollFeeCalculator.Test
{
    public class SwedenTollCalculatorTests
    {
        [Fact]
        public void DateIsWeekend()
        {
            var calculator = new SwedenTollCalculator();
            var weekend = DateTime.ParseExact("26/12/2021 06:00", "dd/MM/yyyy HH:mm", null); // Sunday

            var result = calculator.IsTollFree(weekend);

            Assert.True(result);
        }

        [Fact]
        public void DateIsHoliday()
        {
            var calculator = new SwedenTollCalculator();
            var holiday = DateTime.ParseExact("31/12/2021 06:00", "dd/MM/yyyy HH:mm", null); // New year's eve

            var result = calculator.IsTollFree(holiday);

            Assert.True(result);
        }

        [Fact]
        public void DateIsNotTollFree()
        {
            var calculator = new SwedenTollCalculator();
            var weekday = DateTime.ParseExact("29/12/2021 06:00", "dd/MM/yyyy HH:mm", null); // Wednesday

            var result = calculator.IsTollFree(weekday);

            Assert.False(result);
        }

        [Fact]
        public void NightIsZeroFee()
        {
            var calculator = new SwedenTollCalculator();
            var early = DateTime.ParseExact("29/12/2021 05:59", "dd/MM/yyyy HH:mm", null); // Wednesday
            var later = DateTime.ParseExact("29/12/2021 18:30", "dd/MM/yyyy HH:mm", null); // Wednesday

            var resultEarly = calculator.GetFeeForTime(early);
            var resultLater = calculator.GetFeeForTime(later);

            Assert.Equal<uint>(0, resultEarly);
            Assert.Equal<uint>(0, resultLater);
        }

        [Fact]
        public void DayIsNotZeroFee()
        {
            var calculator = new SwedenTollCalculator();
            var early = DateTime.ParseExact("29/12/2021 06:00", "dd/MM/yyyy HH:mm", null); // Wednesday
            var later = DateTime.ParseExact("29/12/2021 18:29", "dd/MM/yyyy HH:mm", null); // Wednesday

            var resultEarly = calculator.GetFeeForTime(early);
            var resultLater = calculator.GetFeeForTime(later);

            Assert.NotEqual<uint>(0, resultEarly);
            Assert.NotEqual<uint>(0, resultLater);
        }

        [Fact]
        public void EmergencyIsTollFreeOnWeekday()
        {
            var calculator = new SwedenTollCalculator();
            var weekday = DateTime.ParseExact("29/12/2021 06:15", "dd/MM/yyyy HH:mm", null); // Wednesday
            var emergency = new Vehicle(VehicleType.Emergency);

            var resultForDay = calculator.IsTollFree(weekday);
            var resultForVehicle = calculator.IsTollFree(emergency);
            var resultFee = calculator.GetFee(emergency, new List<DateTime> { weekday });

            Assert.False(resultForDay);
            Assert.True(resultForVehicle);
            Assert.Equal<uint>(0, resultFee);
        }

        [Fact]
        public void CarHasFeeOnWeekday()
        {
            var calculator = new SwedenTollCalculator();
            var weekday = DateTime.ParseExact("29/12/2021 06:15", "dd/MM/yyyy HH:mm", null); // Wednesday
            var car = new Vehicle(VehicleType.Car);

            var resultForDay = calculator.IsTollFree(weekday);
            var resultForVehicle = calculator.IsTollFree(car);
            var resultFee = calculator.GetFee(car, new List<DateTime> { weekday });

            Assert.False(resultForDay);
            Assert.False(resultForVehicle);
            Assert.NotEqual<uint>(0, resultFee);
        }

        [Fact]
        public void DatesInHourTimeframeOnWeekday()
        {
            var calculator = new SwedenTollCalculator();
            var dates = new List<DateTime>
            {
                DateTime.ParseExact("29/12/2021 06:15", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 06:20", "dd/MM/yyyy HH:mm", null) // Fee is 9
            };
            var car = new Vehicle(VehicleType.Car);
            
            var resultForVehicle = calculator.IsTollFree(car);
            var resultFee = calculator.GetFee(car, dates);

            Assert.False(resultForVehicle);
            Assert.Equal<uint>(9, resultFee);
        }

        [Fact]
        public void DatesOutsideHourTimeframeOnWeekday()
        {
            var calculator = new SwedenTollCalculator();
            var dates = new List<DateTime>
            {
                DateTime.ParseExact("29/12/2021 06:15", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 07:15", "dd/MM/yyyy HH:mm", null) // Fee is 22
            };
            var car = new Vehicle(VehicleType.Car);

            var resultForVehicle = calculator.IsTollFree(car);
            var resultFee = calculator.GetFee(car, dates);

            Assert.False(resultForVehicle);
            Assert.Equal<uint>(31, resultFee); 
        }

        [Fact]
        public void NotBiggerThenMaxFeeOnWeekday()
        {
            var calculator = new SwedenTollCalculator();
            var dates = new List<DateTime> // Fee in sum is 92
            {
                DateTime.ParseExact("29/12/2021 06:00", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 07:00", "dd/MM/yyyy HH:mm", null), // Fee is 22
                DateTime.ParseExact("29/12/2021 08:00", "dd/MM/yyyy HH:mm", null), // Fee is 16
                DateTime.ParseExact("29/12/2021 09:00", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 10:00", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 11:00", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 12:00", "dd/MM/yyyy HH:mm", null), // Fee is 9
                DateTime.ParseExact("29/12/2021 13:00", "dd/MM/yyyy HH:mm", null), // Fee is 9
            };
            var car = new Vehicle(VehicleType.Car);

            var resultForVehicle = calculator.IsTollFree(car);
            var resultFee = calculator.GetFee(car, dates);

            Assert.False(resultForVehicle);
            Assert.Equal<uint>(60, resultFee);
        }
    }
}
