using Project2API.Application.Repositories;
using Project2API.Persistence.Contexts;
using Project2API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2API.Persistence.Repositories
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(Project2APIDbContext context) : base(context)
        {
        }
    }
}
