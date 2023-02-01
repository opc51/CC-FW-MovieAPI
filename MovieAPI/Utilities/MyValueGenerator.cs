using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MovieAPI.Repository;
using System.Linq;

namespace MovieAPI
{
    /// <summary>
    /// https://social.technet.microsoft.com/wiki/contents/articles/53858.entity-framework-core-hasvaluegenerator-using-c.aspx
    /// </summary>
    public class MyValueGenerator : ValueGenerator
    {
        private readonly APIContext _context;

        public MyValueGenerator(APIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool GeneratesTemporaryValues => false;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected override object NextValue(EntityEntry entry)
        {
            var lastId = _context.Movies
                                      .AsNoTracking()
                                      .FirstOrDefault()?.Id;
            return lastId + 1;
        }
    }
}
