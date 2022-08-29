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
    public class BookService : IService<BookDTO>
    {
        private BookRepository _bookRepository;
        private AuthorService _authorService;
        private PublisherService _publisherService;
        private ActionService _actionService;
        public BookService(string connectionString)
        {
            _bookRepository = new BookRepository(connectionString);
            _authorService = new AuthorService(connectionString);
            _publisherService = new PublisherService(connectionString);
            _actionService = new ActionService(connectionString);
        }
        public void Create(BookDTO item)
        {
            _bookRepository.Create(ConvertBookDTOToBook(item));
        }

        public void Delete(int? id)
        {
            _bookRepository.Delete(id);
        }

        public BookDTO Find(int? id)
        {
            return ConvertBookToBookDTO(_bookRepository.Find(id));
        }

        public IEnumerable<BookDTO> FindAll(string value)
        {
            return ConvertBookToBookDTO(_bookRepository.FindAll(value));
        }

        public IEnumerable<BookDTO> GetAll()
        {
            return ConvertBookToBookDTO(_bookRepository.GetAll());
        }

        public void Update(BookDTO item)
        {
            _bookRepository.Update(ConvertBookDTOToBook(item));
        }

        public BookDTO ConvertBookToBookDTO(Book book)
        {
            if (book != null)
            {
                var bookDTO = new BookDTO()
                {
                    Id = book.Id,
                    BookName = book.BookName,
                    CountPage = book.CountPage,
                    Style = book.Style,
                    Price = book.Price,
                    Count = book.Count,
                    Year = book.Year
                };
                bookDTO.AutorBook = _authorService.Find(book.AutorBook.Id);
                bookDTO.PublisherBook = _publisherService.Find(book.PublisherBook.Id);
                try
                {
                    if (book.ActionBook == null || _actionService.Find(book.ActionBook.Id) == null)
                        throw new Exception();
                    if (book.ActionBook.End <= DateTime.Now)
                    {
                        _actionService.Delete(book.ActionBook.Id);
                        throw new Exception();
                    }
                    bookDTO.ActionBook = _actionService.Find(book.ActionBook.Id);
                }
                catch
                {
                    bookDTO.ActionBook = null;
                }
                return bookDTO;
            }
            return null;
        }

        public IEnumerable<BookDTO> ConvertBookToBookDTO(IEnumerable<Book> books)
        {
            if (books != null)
            {
                var tempBooksDTO = new List<BookDTO>();
                foreach (var item in books)
                    tempBooksDTO.Add(ConvertBookToBookDTO(item));
                return tempBooksDTO;
            }
            return null;
        }

        public Book ConvertBookDTOToBook(BookDTO bookDTO)
        {
            if (bookDTO != null)
            {
                var book = new Book()
                {
                    Id = bookDTO.Id,
                    BookName = bookDTO.BookName,
                    CountPage = bookDTO.CountPage,
                    Style = bookDTO.Style,
                    Price = bookDTO.Price,
                    Count = bookDTO.Count,
                    Year = bookDTO.Year
                };
                book.AutorBook = _authorService.ConvertAuthorDTOToAuthor(bookDTO.AutorBook);
                book.PublisherBook = _publisherService.ConvertPublisherDTOToPublisher(bookDTO.PublisherBook);
                try
                {
                    if (bookDTO.ActionBook == null || _actionService.Find(bookDTO.ActionBook.Id) == null)
                        throw new Exception();
                    book.ActionBook = _actionService.ConvertActionDTOToAction(bookDTO.ActionBook);
                }
                catch
                {
                    book.ActionBook = null;
                }
                return book;
            }
            return null;
        }
    }
}
