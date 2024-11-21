using Project2API.Application.Repositories;
using Project2API.Domain.Entities;
using Project2API.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2API.Persistence.Repositories
{
    public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(Project2APIDbContext context) : base(context)
        {
        }
    }
}