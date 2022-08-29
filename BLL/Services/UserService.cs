using BLL.Interfaces;
using BLL.Models;
using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IService<UserDTO>
    {
        private UserRepository _userRepository;
        private PeopleService _peopleService;
        private BookService _bookService;
        public UserService(string connectionString)
        {
            _userRepository = new UserRepository(connectionString);
            _peopleService = new PeopleService(connectionString);
            _bookService = new BookService(connectionString);
        }
        public bool AuthenticateUser(NetworkCredential credential)
        {
            var tempUser = _userRepository.Find(credential.UserName);
            if (tempUser == null)
                return false;

            if (tempUser.Password != credential.Password)
                return false;

            return true;
        }

        public void Create(UserDTO item)
        {
            _userRepository.Create(ConvertUserDTOToUser(item));
        }

        public void Delete(int? id)
        {
            _userRepository.Delete(id);
        }

        public UserDTO Find(int? id)
        {
            return ConvertUserToUserDTO(_userRepository.Find(id));
        }

        public UserDTO Find(string login)
        {
            return ConvertUserToUserDTO(_userRepository.Find(login));
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return ConvertUserToUserDTO(_userRepository.GetAll());
        }

        public void Update(UserDTO item)
        {
            _userRepository.Update(ConvertUserDTOToUser(item));
        }

        private UserDTO ConvertUserToUserDTO(User user)
        {
            if (user != null)
            {
                var tempUserDTO = new UserDTO()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password,
                    IsAdmin = user.IsAdmin
                };
                tempUserDTO.UserInfo = _peopleService.Find(user.UserInfoId);
                if (user.Books.Count > 0)
                {
                    foreach (var item in user.Books)
                    {
                        tempUserDTO.Books.Add(_bookService.Find(item.Id));
                    }
                }

                return tempUserDTO;
            }
            return null;
        }

        private IEnumerable<UserDTO> ConvertUserToUserDTO(IEnumerable<User> users)
        {
            if (users != null)
            {
                var tempUsersDTO = new List<UserDTO>();
                foreach (var item in users)
                    tempUsersDTO.Add(ConvertUserToUserDTO(item));
                return tempUsersDTO;
            }
            return null;
        }

        private User ConvertUserDTOToUser(UserDTO userDTO)
        {
            if (userDTO != null)
            {
                var tempUser = new User()
                {
                    Id = userDTO.Id,
                    Login = userDTO.Login,
                    Password = userDTO.Password,
                    IsAdmin = userDTO.IsAdmin
                };
                tempUser.UserInfo = new People()
                {
                    Id = userDTO.UserInfo.Id,
                    Name = userDTO.UserInfo.Name,
                    LastName = userDTO.UserInfo.LastName,
                    Email = userDTO.UserInfo.Email
                };
                if (userDTO.Books.Count > 0)
                    foreach (var item in userDTO.Books)
                        tempUser.Books.Add(_bookService.ConvertBookDTOToBook(item));

                return tempUser;
            }
            return null;
        }
    }
}
