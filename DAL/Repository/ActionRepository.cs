using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ActionRepository : IRepositoryBase, IRepository<Models.Action>
    {
        public ActionRepository(string connectionString) : base(connectionString) { }
        public void Create(Models.Action item)
        {
            if (item != null)
            {
                _libraryContext.Actions.Add(item);
                CommitChanges();
            }
        }

        public void Delete(int? id)
        {
            var tempAction = _libraryContext.Actions.Find(id);
            if (tempAction != null)
            {
                foreach (var item in _libraryContext.Books.Where(b => b.ActionBook.Id == tempAction.Id))
                    item.ActionBook = null;
                _libraryContext.Actions.Remove(tempAction);
                CommitChanges();
            }
        }

        public Models.Action Find(int? id)
        {
            return _libraryContext.Actions.AsNoTracking().First(x=>x.Id==id);
        }

        public IEnumerable<Models.Action> Find(decimal discount)
        {
            return _libraryContext.Actions.AsNoTracking().Where(x=>x.Discount == discount);
        }

        public IEnumerable<Models.Action> GetAll()
        {
            return _libraryContext.Actions.AsNoTracking();
        }

        public void Update(Models.Action item)
        {
            var tempAction = _libraryContext.Actions.Find(item.Id);
            tempAction.Discount = item.Discount;
            tempAction.Start = item.Start;
            tempAction.End = item.End;
            CommitChanges();
        }
    }
}
