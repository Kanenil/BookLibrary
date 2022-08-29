using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        public User() { Books = new List<Book>(); }
        public int Id { get; set; }
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string Login { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int UserInfoId { get; set; }
        public virtual People UserInfo { get; set; }
        public virtual IList<Book> Books { get; set; }
    }
}
