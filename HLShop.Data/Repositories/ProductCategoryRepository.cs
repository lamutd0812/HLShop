using HLShop.Data.Infrastructure;
using HLShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace HLShop.Data.Repositories
{
    public interface IProductCategoryRepository
    {
        IEnumerable<ProductCategory> getByAlias(string alias);

        // Method nay ko co san trong tap methods cua RepositoryBase, phai tu dinh nghia
    }

    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        // trien khai method getByAlias
        public IEnumerable<ProductCategory> getByAlias(string alias)
        {
            return this.DbContext.ProductCategories.Where(x => x.Alias == alias);
        }
    }
}