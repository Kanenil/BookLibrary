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
    public class PublisherService : IService<PublisherDTO>
    {
        private PublisherRepository _publisherRepository;
        public PublisherService(string connectionString)
        {
            _publisherRepository = new PublisherRepository(connectionString);
        }
        public void Create(PublisherDTO item)
        {
            _publisherRepository.Create(ConvertPublisherDTOToPublisher(item));
        }

        public void Delete(int? id)
        {
            _publisherRepository.Delete(id);
        }

        public PublisherDTO Find(int? id)
        {
            return ConvertPublisherToPublisherDTO(_publisherRepository.Find(id));
        }

        public IEnumerable<PublisherDTO> FindAll(string value)
        {
            return ConvertPublisherToPublisherDTO(_publisherRepository.FindAll(value));
        }

        public IEnumerable<PublisherDTO> GetAll()
        {
            return ConvertPublisherToPublisherDTO(_publisherRepository.GetAll());
        }

        public void Update(PublisherDTO item)
        {
            _publisherRepository.Update(ConvertPublisherDTOToPublisher(item));
        }

        public PublisherDTO ConvertPublisherToPublisherDTO(Publisher publisher)
        {
            if (publisher != null)
            {
                return new PublisherDTO()
                {
                    Id = publisher.Id,
                    Name = publisher.Name
                };
            }
            return null;
        }

        public IEnumerable<PublisherDTO> ConvertPublisherToPublisherDTO(IEnumerable<Publisher> publishers)
        {
            if (publishers != null)
            {
                var tempUsersDTO = new List<PublisherDTO>();
                foreach (var item in publishers)
                    tempUsersDTO.Add(ConvertPublisherToPublisherDTO(item));
                return tempUsersDTO;
            }
            return null;
        }

        public Publisher ConvertPublisherDTOToPublisher(PublisherDTO publisherDTO)
        {
            if (publisherDTO != null)
            {
                return new Publisher()
                {
                    Id = publisherDTO.Id,
                    Name = publisherDTO.Name
                };
            }
            return null;
        }
    }
}
