using HLShop.Data.Infrastructure;
using HLShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace HLShop.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Posts
                        join pt in DbContext.PostTags
                            on p.ID equals pt.PostID
                        where pt.TagID == tag && p.Status
                        orderby p.CreatedDate descending
                        select p; // lay cac post theo tag truyen vao

            totalRow = query.Count(); // tra ve so luong ban ghi

            query = query.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            // phan trang: lay tu vi tri (pageIndex-1)*pageSize toi vi tri pageSize

            return query;
        }
    }
}