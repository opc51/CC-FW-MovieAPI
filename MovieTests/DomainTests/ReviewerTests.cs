using AutoFixture;
using MovieAPI.Models.Domain;
using System;
using Xunit;

namespace MovieTests.DomainTests
{
    public class ReviewerTests
    {
        private Fixture _fixture = new();

        [Theory]
        [InlineData("123456@.")]
        [InlineData("@abc.com")]
        public void ReviewerCreated_InvalidEmailAddress_ThrowFormatException(string emailAddress)
        {
            Assert.Throws<FormatException>(() => Reviewer.Create(_fixture.Create<string>(), emailAddress, "gb", "01234875456"));

        }

        [Theory]
        [InlineData("john@freewheel.com")]
        [InlineData("john.jones@freewheel.com")]
        [InlineData("john.jones@freewheel.internal.co.uk")]
        public void ReviewerCreated_ValidEmailAddress_CreatesTheInstance(string emailAddress)
        {
            var name = _fixture.Create<string>();
            var sut = Reviewer.Create(name, emailAddress, "gb", "01234875456");
            Assert.Equal(emailAddress, sut.Email);
            Assert.Equal(name, sut.Name);
        }

        [Fact]
        public void EmailAddress_CannotBeUpdated_ToInvalidEmail()
        {
            var emailAddress = "john.jones@freewheel.com";
            var sut = Reviewer.Create(_fixture.Create<string>(), emailAddress, "gb", "01234875456");
            Assert.Equal(emailAddress, sut.Email);

            Assert.Throws<FormatException>(() => { sut.Email = _fixture.Create<string>(); });
        }


        [Theory]
        [InlineData("XP")]
        [InlineData("67")]
        public void Region_InvalidCountryCodes_ThrowsException(string countryCode)
        {
            var name = _fixture.Create<string>();
            Assert.Throws<ArgumentException>(() => Reviewer.Create(name, "joe@joe.com", countryCode, "01234875456"));
        }


        [Theory]
        [InlineData("GB")]
        [InlineData("gb")]
        [InlineData("Gb")]
        public void Region_ValidCountryCodes(string countryCode)
        {
            var name = _fixture.Create<string>();
            var sut = Reviewer.Create(name, "joe@joe.com", countryCode, "01234875456");
            Assert.Equal(countryCode.ToUpper(), sut.Region);
        }

        [Theory]
        [InlineData("11")]
        public void PhoneNumber_InvalidNumber_ThrowsArgumentException(string phoneNumber)
        {
            var name = _fixture.Create<string>();
            Assert.Throws<ArgumentException>(() => { Reviewer.Create(name, "john@freewheel.com", "gb", phoneNumber); });
        }


        [Theory]
        [InlineData("abcdefg")]
        public void PhoneNumber_InvalidLetters_ThrowsException(string phoneNumber)
        {
            var name = _fixture.Create<string>();
            Assert.Throws<com.google.i18n.phonenumbers.NumberParseException>(() => { Reviewer.Create(name, "john@freewheel.com", "gb", phoneNumber); });
        }

        [Theory]
        [InlineData("GB", "1236543569", 44)]
        [InlineData("US", "3333334444", 1)]
        [InlineData("FR", "123456789", 33)]
        public void PhoneNumbers_SuccessfulFor_EachCountry(string countryCode, string phoneNumber, int countryInt)
        {
            var name = _fixture.Create<string>();
            var sut = Reviewer.Create(name, "joe@joe.com", countryCode, phoneNumber);
            Assert.Equal($"{countryInt}{phoneNumber}",sut.PhoneNumber );

        }
    }
}
