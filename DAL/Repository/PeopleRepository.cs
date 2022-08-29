using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PeopleRepository : IRepositoryBase, IRepository<People>
    {
        public PeopleRepository(string connectionString) : base(connectionString) { }

        public void Create(People item)
        {
            if (item != null)
            {
                _libraryContext.Peoples.Add(item);
                CommitChanges();
            }
        }

        public void Delete(int? id)
        {
            var tempPeople = _libraryContext.Peoples.Find(id);
            if (tempPeople != null)
            {
                _libraryContext.Peoples.Remove(tempPeople);
                CommitChanges();
            }
        }

        public People Find(int? id)
        {
            return _libraryContext.Peoples.AsNoTracking().First(x=>x.Id==id);
        }

        public IEnumerable<People> GetAll()
        {
            return _libraryContext.Peoples;
        }

        public void Update(People item)
        {
            var tempPeople = _libraryContext.Peoples.Find(item.Id);
            tempPeople.Name = item.Name;
            tempPeople.LastName = item.LastName;
            tempPeople.Email = item.Email;
            CommitChanges();
        }
    }
}
