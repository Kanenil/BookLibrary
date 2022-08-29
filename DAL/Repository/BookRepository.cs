using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class BookRepository : IRepositoryBase, IRepository<Book>
    {
        public BookRepository(string connectionString) : base(connectionString) { }

        public void Create(Book item)
        {
            if (item != null)
            {
                item.AutorBook = _libraryContext.Autors.Where(x=>x.Name == item.AutorBook.Name && 
                                                              x.LastName == item.AutorBook.LastName && 
                                                              x.Patronymic == item.AutorBook.Patronymic)
                                                       .First();

                item.PublisherBook = _libraryContext.Publishers.Where(x=>x.Name == item.PublisherBook.Name)
                                                               .First();
                if (item.ActionBook != null)
                {
                    item.ActionBook = _libraryContext.Actions.Where(x => x.Start.Day == item.ActionBook.Start.Day &&
                                                                             x.Start.Month == item.ActionBook.Start.Month &&
                                                                             x.End.Day == item.ActionBook.End.Day &&
                                                                             x.End.Month == item.ActionBook.End.Month &&
                                                                             x.Discount == item.ActionBook.Discount)
                                                                 .FirstOrDefault();
                }
                _libraryContext.Books.Add(item);
                CommitChanges();
            }
        }

        public void Delete(int? id)
        {
            var tempBook = Find(id);
            tempBook.PublisherBook = _libraryContext.Publishers.Find(tempBook.PublisherBook.Id);
            if (tempBook.ActionBook != null)
                tempBook.ActionBook = _libraryContext.Actions.Find(tempBook.ActionBook.Id);
            if (tempBook != null)
            {
                try
                {
                    _libraryContext.Books.Attach(tempBook);
                    _libraryContext.Books.Remove(tempBook);
                }
                catch
                {
                    var temp = _libraryContext.ChangeTracker.Entries()
                                                            .Where(x=>x.Entity is Book)
                                                            .Where(x=>(x.Entity as Book).Id == tempBook.Id)
                                                            .First();
                    temp.State = EntityState.Deleted;
                }

                CommitChanges();
            }
        }

        public Book Find(int? id)
        {
            return _libraryContext.Books.AsNoTracking().First(x=>x.Id==id);
        }

        public IEnumerable<Book> FindAll(string value)
        {
            return _libraryContext.Books.AsNoTracking()
                   .Where(b => b.BookName.ToUpper().Contains(value.ToUpper()) ||
                               b.AutorBook.Name.ToUpper().Contains(value.ToUpper()) ||
                               b.AutorBook.LastName.ToUpper().Contains(value.ToUpper()) ||
                               b.AutorBook.Patronymic.ToUpper().Contains(value.ToUpper()) ||
                               b.Style.ToUpper().Contains(value.ToUpper()) ||
                               b.PublisherBook.Name.ToUpper().Contains(value.ToUpper()))
                   .ToList();
        }

        public IEnumerable<Book> GetAll()
        {
            return _libraryContext.Books.AsNoTracking().Include("AutorBook").Include("PublisherBook").Include("ActionBook");
        }

        public void Update(Book item)
        {
            var tempBook = _libraryContext.Books.Find(item.Id);
            tempBook.ActionBook = null;
            tempBook.BookName = item.BookName;
            tempBook.AutorBook = _libraryContext.Autors.Where(x => x.Name == item.AutorBook.Name &&
                                                              x.LastName == item.AutorBook.LastName &&
                                                              x.Patronymic == item.AutorBook.Patronymic)
                                                       .First();
            tempBook.CountPage = item.CountPage;
            tempBook.Price = item.Price;
            tempBook.Count = item.Count;
            tempBook.Style = item.Style;
            tempBook.Year = item.Year;
            tempBook.PublisherBook = _libraryContext.Publishers.Where(x => x.Name == item.PublisherBook.Name)
                                                               .First();
            if (item.ActionBook != null)
            {
                tempBook.ActionBook = _libraryContext.Actions.Where(x => x.Start.Day == item.ActionBook.Start.Day &&
                                                                         x.Start.Month == item.ActionBook.Start.Month &&
                                                                         x.End.Day == item.ActionBook.End.Day &&
                                                                         x.End.Month == item.ActionBook.End.Month &&
                                                                         x.Discount == item.ActionBook.Discount)
                                                             .FirstOrDefault();
            }
            if (tempBook.ActionBook != null && item.ActionBook == null)
            {
                tempBook.ActionBook = null;
            }
            try
            {
                _libraryContext.Entry(tempBook).State = EntityState.Modified;
            }
            catch
            {
                var temp = _libraryContext.ChangeTracker.Entries()
                                                           .Where(x => x.Entity is Book)
                                                           .Where(x => (x.Entity as Book).Id == tempBook.Id)
                                                           .First();
                temp.State = EntityState.Modified;
            }
            CommitChanges();
        }
    }
}
