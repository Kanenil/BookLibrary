using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserDTO
    {
        public UserDTO() { Books = new List<BookDTO>(); }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual PeopleDTO UserInfo { get; set; }
        public virtual IList<BookDTO> Books { get; set; }
    }
}
