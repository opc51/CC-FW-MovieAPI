using AutoFixture;
using MovieAPI.Models.Entities;
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
            Assert.Throws<FormatException>(() => Reviewer.Create(_fixture.Create<string>(), emailAddress));

        }

        [Theory]
        [InlineData("john@freewheel.com")]
        [InlineData("john.jones@freewheel.com")]
        [InlineData("john.jones@freewheel.internal.co.uk")]
        public void ReviewerCreated_ValidEmailAddress_CreatesTheInstance(string emailAddress)
        {
            var name = _fixture.Create<string>();
            var sut = Reviewer.Create(name, emailAddress);
            Assert.Equal(emailAddress, sut.Email);
            Assert.Equal(name, sut.Name);
        }

        [Fact]
        public void EmailAddress_CannotBeUpdated_ToInvalidEmail()
        {
            var emailAddress = "john.jones@freewheel.com";
            var sut = Reviewer.Create(_fixture.Create<string>(), emailAddress);
            Assert.Equal(emailAddress, sut.Email);

            Assert.Throws<FormatException>(() => { sut.Email = _fixture.Create<string>(); });
        }
    }
}
