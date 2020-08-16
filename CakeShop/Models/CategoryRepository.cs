using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CakeShop.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _appDbContext;

        public CategoryRepository(ApplicationDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Category> AllCategories => _appDbContext.Categories;

    }
}
