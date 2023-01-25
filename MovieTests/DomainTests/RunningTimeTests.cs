using MovieAPI.Models.Entities.Common;
using System;
using Xunit;

namespace MovieTests.DomainTests
{
    public class RunningTimeTests
    {
        [Fact]
        public void ImplicityConversion_FromRunningTimeToInt_Sucessful()
        {
            int timing = 183;
            RunningTime runningTime = RunningTime.Create(timing);
            int sut = (int) runningTime;
            Assert.Equal(typeof(int), sut.GetType());
        }

        [Fact]
        public void ExplicitConversion_FromIntToRunningTime_Successful()
        {
            int year = 183;
            RunningTime sut = (RunningTime)year;
            Assert.Equal(typeof(RunningTime), sut.GetType());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(183)]
        [InlineData(1440)]
        public void CreatesRunningTime_WithValidIntegerData_IsSuccessful(int length)
        {
            var sut = RunningTime.Create(length);
            Assert.Equal(length, sut.Value);
        }

        [Theory]
        [InlineData(0.1, 6)]
        [InlineData(3.25, 195)]
        [InlineData(24, 1440)]
        public void CreatesRunningTime_WithValidFloatData_IsSuccessful(float length, int resultInMinutes)
        {
            var sut = RunningTime.Create(length);
            Assert.Equal(resultInMinutes, sut.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1441)]
        public void ThrowsException_WhenIntegerData_IsValid(int timing)
        {
            Assert.Throws<ArgumentException>(() => RunningTime.Create(timing));
        }

        [Theory]
        [InlineData(0.001)]
        [InlineData(24.1)]
        public void ThrowsException_WhenFloatData_IsValid(float timing)
        {
            Assert.Throws<ArgumentException>(() => RunningTime.Create(timing));
        }
    }
}
