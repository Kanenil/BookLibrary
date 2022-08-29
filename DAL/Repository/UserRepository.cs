using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository : IRepositoryBase, IRepository<User>
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        public void Create(User item)
        {
            if (item != null)
            {
                _libraryContext.Users.Add(item);
                CommitChanges();
            }
        }

        public void Delete(int? id)
        {
            var tempUser = _libraryContext.Users.Find(id);
            if (tempUser != null)
            {
                _libraryContext.Users.Remove(tempUser);
                _libraryContext.Peoples.Remove(_libraryContext.Peoples.Find(tempUser.UserInfoId));
                CommitChanges();
            }
        }

        public User Find(int? id)
        {
            return _libraryContext.Users.AsNoTracking().First(x => x.Id == id);
        }

        public User Find(string login)
        {
            try
            {
                return _libraryContext.Users.AsNoTracking().First((x) => x.Login == login);
            }
            catch 
            {
                return null;
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _libraryContext.Users.AsNoTracking().Include("Books");
        }

        public void Update(User item)
        {
            var tempUser = Find(item.Id);
            tempUser.Login = item.Login;
            tempUser.Password = item.Password;
            tempUser.IsAdmin = item.IsAdmin;
            tempUser.UserInfo.Name = item.UserInfo.Name;
            tempUser.UserInfo.LastName = item.UserInfo.LastName;
            tempUser.UserInfo.Email = item.UserInfo.Email;
            Book addedBook = null;
            foreach (var book in item.Books)
            {
                if(book.ActionBook != null)
                    book.ActionBook = _libraryContext.Actions.Where(x => x.Start.Day == book.ActionBook.Start.Day &&
                                                                         x.Start.Month == book.ActionBook.Start.Month &&
                                                                         x.End.Day == book.ActionBook.End.Day &&
                                                                         x.End.Month == book.ActionBook.End.Month)
                                                              .First();
                if (book.AutorBook != null)
                    book.AutorBook = _libraryContext.Autors.Where(x => x.Name == book.AutorBook.Name &&
                                                              x.LastName == book.AutorBook.LastName &&
                                                              x.Patronymic == book.AutorBook.Patronymic)
                                                       .First();
                if (book.PublisherBook != null)
                    book.PublisherBook = _libraryContext.Publishers.Where(x=> x.Name == book.PublisherBook.Name)
                                                                   .FirstOrDefault();


                if (tempUser.Books.FirstOrDefault(x => x.BookName == book.BookName &&
                                            x.Style == book.Style &&
                                            x.CountPage == book.CountPage &&
                                            x.Price == book.Price) == null)
                {
                    tempUser.Books.Add(book);
                    addedBook = book;
                    _libraryContext.Entry(book).State = EntityState.Added;
                }
            }
            try
            {
                _libraryContext.Entry(tempUser).State = EntityState.Modified;
            }
            catch
            {
                var temp = _libraryContext.ChangeTracker.Entries()
                                                            .Where(x => x.Entity is User)
                                                            .Where(x => (x.Entity as User).Id == tempUser.Id)
                                                            .First();
                if (addedBook != null)
                {
                    (temp.Entity as User).Books.Add(tempUser.Books.Last());
                }
                temp.State = EntityState.Modified;
            }
            CommitChanges();
        }
    }
}
