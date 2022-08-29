using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PublisherRepository : IRepositoryBase, IRepository<Publisher>
    {
        public PublisherRepository(string connectionString) : base(connectionString) { }

        public void Create(Publisher item)
        {
            if (item != null)
            {
                _libraryContext.Publishers.Add(item);
                CommitChanges();
            }
        }

        public void Delete(int? id)
        {
            var tempPublisher = _libraryContext.Publishers.Find(id);
            if (tempPublisher != null)
            {
                _libraryContext.Publishers.Remove(tempPublisher);
                CommitChanges();
            }
        }

        public Publisher Find(int? id)
        {
            return _libraryContext.Publishers.AsNoTracking().First(x=>x.Id==id);
        }

        public IEnumerable<Publisher> FindAll(string value)
        {
            return _libraryContext.Publishers.AsNoTracking().Where(x=>x.Name.ToUpper().Contains(value.ToUpper()));
        }

        public IEnumerable<Publisher> GetAll()
        {
            return _libraryContext.Publishers.AsNoTracking();
        }

        public void Update(Publisher item)
        {
            var tempPublisher = _libraryContext.Publishers.Find(item.Id);
            tempPublisher.Name = item.Name;
            CommitChanges();
        }
    }
}
