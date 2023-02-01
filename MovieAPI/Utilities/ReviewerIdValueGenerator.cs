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
    public class ReviewerIdValueGenerator : ValueGenerator<int>
    {
        private readonly APIContext _context;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ReviewerIdValueGenerator(APIContext context)
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
        /// <returns></returns>
        protected int NextReviewerId() { 
            var lastId = _context.Reviewers
                                      .AsNoTracking()
                                      .FirstOrDefault()?.Id;

            return lastId == null ? 1 : lastId.Value + 1;
        }

        /// <summary>
        /// Template method to perform value generation for AccountNumber.
        /// </summary>
        /// <param name="entry">In this case Customer</param>
        /// <returns>Next Movie Id/returns>
        public override int Next(EntityEntry entry) => NextReviewerId();

        /// <summary>
        /// Gets a value to be assigned to AccountNumber property
        /// </summary>
        /// <param name="entry">In this case Customer</param>
        /// <returns>Movie Id</returns>
        protected override object NextValue(EntityEntry entry) => NextReviewerId();
    }
}
