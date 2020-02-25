using HLShop.Data.Infrastructure;
using HLShop.Model.Models;

namespace HLShop.Data.Repositories
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {

    }

    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}