using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public abstract class IRepositoryBase
    {
        protected LibraryContext _libraryContext;
        public IRepositoryBase(string connectionString)
        {
            _libraryContext = new LibraryContext(connectionString);
        }
        protected void CommitChanges()
        {
            _libraryContext.SaveChanges();
        }
    }
}
