using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Domain = MovieAPI.Models.Domain;

namespace MovieTests
{
    public class LinqExceptionTests
    {
        [Fact]
        public void SelectOn_AsEnumerableList_Produces_emptyList()
        {
            var sut = new List<Domain.Movie>().AsEnumerable();
            var result = sut.Select(x => x);
            Assert.Empty(result);
        }

        [Fact]
        public void WhereOn_AsEnumerableList_Produces_emptyList()
        {
            var sut = new List<Domain.Movie>().AsEnumerable();
            var result = sut.Where(x => x.Id == 100);
            Assert.Empty(result);
        }

        #region First()
        [Fact]
        public void First_onNull_ArgumentNullExceptionException()
        {
            //Arrange
            List<Domain.Movie> _sut = null;

            //Act Assert
            Assert.Throws<ArgumentNullException>(() => _sut.First());
        }

        [Fact]
        public void First_onEmptyList_ThrowsInvalidOperationException()
        {
            //Arrange
            List<Domain.Movie> _sut = new();

            //Act Assert
            Assert.Throws<InvalidOperationException>(() => _sut.First());
        }
        #endregion


        [Fact]
        public void Where_onNull_ArgumentNullExceptionException()
        {
            //Arrange
            List<Domain.Movie> _sut = null;

            //Act Assert
            Assert.Throws<ArgumentNullException>(() => _sut.Where(x => x.Id == x.RunningTime.Value));
        }

        #region FirstOrDefault
        [Fact]
        public void FirstOrDefault_onNull_ArgumentNullExceptionException()
        {
            //Arrange
            List<Domain.Movie> _sut = null;

            //Act Assert
            Assert.Throws<ArgumentNullException>(() => _sut.FirstOrDefault());
        }

        [Fact]
        public void FirstorDefault_onEmptyList_ThrowsInvalidOperationException()
        {
            //Arrange
            List<Domain.Movie> _sut = new();

            //Act Assert
            //Assert.Empty(_sut.FirstOrDefault());
        }

        //[Fact]
        //public void FirstOrDefault_onNullableClass_ThrowsInvalidOperationException()
        //{
        //    //Arrange
        //    List<Entities.Movie?> _sut = new();

        //    //Act Assert
        //    Assert.Throws<InvalidOperationException>(() => _sut.FirstOrDefault());
        //}

        #endregion



        [Fact]
        public void ToList_onEmptyList_GivesAnEmptyList()
        {
            //Arrange
            List<Domain.Movie> _sut = new();

            //Act Assert
            Assert.Empty(_sut.ToList());
        }


        [Fact]
        public void ToList_onNull_ThrowsInvalidOperationException()
        {
            //Arrange
            List<Domain.Movie> _sut = null;

            //Act Assert
            Assert.Throws<ArgumentNullException>(() => _sut.ToList());
        }

        private class FinancialOrder
        {
            public int Id { get; set; }
            public int ConvertedOrderId { get; set; }
        }


        [Fact]
        public void Testing_stuff()
        {
            List<FinancialOrder> financeOrders = new()
            {
                new FinancialOrder()
                {
                    Id = 1,
                    ConvertedOrderId = 300
                },
                new FinancialOrder()
                {
                    Id = 1,
                    ConvertedOrderId = 301
                }
            };
            // fsOrders.Where(x => x.ConvertedOrderId == orderTransfer.ExternalId).First().Insertions;
            Assert.Throws<InvalidOperationException>(() => financeOrders.Where(x => x.ConvertedOrderId == 10000).First().Id);
        }


        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void ChangeClass_Works()
        {
            var person = new Person() { Age = 21, Name = "Joe" };

            Assert.True(string.Equals("Joe", person.Name));

            ChangeName(person);

            Assert.True(string.Equals("david", person.Name));
        }


        private void ChangeName(Person person)
        {
            person.Name = "david";
        }

        //var currentInsertions = 
    }
}
