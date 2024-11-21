using Project2API.Application.Repositories;
using Project2API.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2API.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<Project2API.Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(Project2APIDbContext context) : base(context)
        {
        }
    }
}
