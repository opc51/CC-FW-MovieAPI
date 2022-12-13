//using MovieAPI.Models.Entities.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;


//namespace MovieTests.DomainTests
//{
//    public class EmailAddressTests
//    {
//        [Theory]
//        [InlineData("")]
//        [InlineData((string)null)]
//        public void EmailAddress_BlankOrNUllString_ThrowsArgumentException(string email)
//        {
//            Assert.Throws<ArgumentException>(() => EmailAddress.Create(email));
//        }

//        [Fact]
//        public void EmailAddressConvertsToStringImplicitly()
//        {
//            EmailAddress emailAdressClass = EmailAddress.Create("joe@freewheel.com");
//            string emailAddressString = emailAdressClass;

//            Assert.Equal(typeof(EmailAddress), emailAdressClass.GetType());
//            Assert.Equal(typeof(string), emailAddressString.GetType());
//        }


//        [Fact]
//        public void StringConvertsToEmailAddressImplicitly()
//        {
//            string emailAddressString = "john@freewheel.tv";
//            EmailAddress emailAdressClass = emailAddressString;

//            Assert.Equal(typeof(EmailAddress), emailAdressClass.GetType());
//            Assert.Equal(typeof(string), emailAddressString.GetType());
//        }
//    }
//}
