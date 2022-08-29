using BLL.Interfaces;
using BLL.Models;
using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthorService : IService<AuthorDTO>
    {
        private AuthorRepository _authorRepository;
        public AuthorService(string connectionString)
        {
            _authorRepository = new AuthorRepository(connectionString);
        }

        public void Create(AuthorDTO item)
        {
            _authorRepository.Create(ConvertAuthorDTOToAuthor(item));
        }

        public void Delete(int? id)
        {
            _authorRepository.Delete(id);
        }

        public AuthorDTO Find(int? id)
        {
            return ConvertAuthorToAuthorDTO(_authorRepository.Find(id));
        }

        public IEnumerable<AuthorDTO> FindAll(string value)
        {
            return ConvertAuthorToAuthorDTO(_authorRepository.FindAll(value));
        }

        public IEnumerable<AuthorDTO> GetAll()
        {
            return ConvertAuthorToAuthorDTO(_authorRepository.GetAll());
        }

        public void Update(AuthorDTO item)
        {
            _authorRepository.Update(ConvertAuthorDTOToAuthor(item));
        }

        public AuthorDTO ConvertAuthorToAuthorDTO(Author author)
        {
            if (author != null)
            {
                return new AuthorDTO()
                {
                    Id = author.Id,
                    Name = author.Name,
                    LastName = author.LastName,
                    Patronymic = author.Patronymic
                };
            }
            return null;
        }

        public IEnumerable<AuthorDTO> ConvertAuthorToAuthorDTO(IEnumerable<Author> authors)
        {
            if (authors != null)
            {
                var tempAuthorsDTO = new List<AuthorDTO>();
                foreach (var item in authors)
                    tempAuthorsDTO.Add(ConvertAuthorToAuthorDTO(item));
                return tempAuthorsDTO;
            }
            return null;
        }

        public Author ConvertAuthorDTOToAuthor(AuthorDTO authorDTO)
        {
            if (authorDTO != null)
            {
                return new Author()
                {
                    Id = authorDTO.Id,
                    Name = authorDTO.Name,
                    LastName = authorDTO.LastName,
                    Patronymic = authorDTO.Patronymic      
                };
            }
            return null;
        }
    }
}
