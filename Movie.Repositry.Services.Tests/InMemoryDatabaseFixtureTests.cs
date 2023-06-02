﻿using Movie.Repository.Services.Tests.ContextSharing;

namespace Movie.Repositry.Services.Tests
{
    public class InMemoryDatabaseFixtureTests
    {
        [Fact]
        public void InMemoryDatabaseFixture_LoadsData()
        {
            var fixture = new InMemoryDatabaseFixture();
            fixture._database.Movies.Count().Should().BeGreaterThan(0);
        }
    }
}
