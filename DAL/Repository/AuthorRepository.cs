using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AuthorRepository : IRepositoryBase, IRepository<Author>
    {
        public AuthorRepository(string connectionString) : base(connectionString) { }

        public void Create(Author item)
        {
            if (item != null)
            {
                _libraryContext.Autors.Add(item);
                CommitChanges();
            }
        }

        public void Delete(int? id)
        {
            var tempAuthor = _libraryContext.Autors.Find(id);
            if (tempAuthor != null)
            {
                _libraryContext.Autors.Remove(tempAuthor);
                CommitChanges();
            }
        }

        public Author Find(int? id)
        {
            return _libraryContext.Autors.AsNoTracking().First(x=>x.Id==id);
        }

        public IEnumerable<Author> FindAll(string value)
        {
            return _libraryContext.Autors.AsNoTracking()
                   .Where(b => b.Name.ToUpper().Contains(value.ToUpper()) ||
                               b.LastName.ToUpper().Contains(value.ToUpper()) ||
                               b.Patronymic.ToUpper().Contains(value.ToUpper()))
                   .ToList();
        }

        public IEnumerable<Author> GetAll()
        {
            return _libraryContext.Autors.AsNoTracking();
        }

        public void Update(Author item)
        {
            var tempAuthor = _libraryContext.Autors.Find(item.Id);
            tempAuthor.Name = item.Name;
            tempAuthor.LastName = item.LastName;
            tempAuthor.Patronymic = item.Patronymic;
            CommitChanges();
        }
    }
}
