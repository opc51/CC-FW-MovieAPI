using MovieAPI.Models.Enum;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MovieTests.DomainTests
{
    public class GenreTests
    {
        [Fact]
        public void IdentifiesHeros_Successfully()
        {
            var heroList = new List<GenreType>()
            {
                GenreType.SuperHero,
                GenreType.Hero,
                GenreType.Heros,
                GenreType.Marvel,
                GenreType.DC,
                GenreType.Funny,
                GenreType.Weepy
            };

            var numberOfHeroes = heroList.AsEnumerable()
                                .Where(x => x.Equals(GenreType.SuperHero))
                                .Count();

            var numberOfNonHeroes = heroList.AsEnumerable()
                    .Where(x => !x.Equals(GenreType.SuperHero))
                    .Count();

            Assert.Equal(heroList.Count - numberOfNonHeroes, numberOfHeroes);
        }

        [Fact]
        public void GenreTypes_Worksas_expected()
        {
            var myenum = GenreType.FromName("SuperHero");
            Assert.Equal(myenum, GenreType.SuperHero);
        }
    }
}
