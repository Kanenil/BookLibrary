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
    public class PeopleService : IService<PeopleDTO>
    {
        private PeopleRepository _peopleRepository;
        public PeopleService(string connectionString)
        {
            _peopleRepository = new PeopleRepository(connectionString);
        }
        public void Create(PeopleDTO item)
        {
            _peopleRepository.Create(ConvertPeopleDTOToPeople(item));
        }

        public void Delete(int? id)
        {
            _peopleRepository.Delete(id);
        }

        public PeopleDTO Find(int? id)
        {
            return ConvertPeopleToPeopleDTO(_peopleRepository.Find(id));
        }

        public IEnumerable<PeopleDTO> GetAll()
        {
            return ConvertPeopleToPeopleDTO(_peopleRepository.GetAll());
        }

        public void Update(PeopleDTO item)
        {
            _peopleRepository.Update(ConvertPeopleDTOToPeople(item));
        }

        private PeopleDTO ConvertPeopleToPeopleDTO(People people)
        {
            if (people != null)
            {
                return new PeopleDTO()
                {
                    Id = people.Id,
                    Name = people.Name,
                    LastName = people.LastName,
                    Email = people.Email
                };
            }
            return null;
        }

        private IEnumerable<PeopleDTO> ConvertPeopleToPeopleDTO(IEnumerable<People> peoples)
        {
            if (peoples != null)
            {
                var tempPeoplesDTO = new List<PeopleDTO>();
                foreach (var item in peoples)
                    tempPeoplesDTO.Add(ConvertPeopleToPeopleDTO(item));
                return tempPeoplesDTO;
            }
            return null;
        }

        private People ConvertPeopleDTOToPeople(PeopleDTO peopleDTO)
        {
            if (peopleDTO != null)
            {

                return new People()
                {
                    Id = peopleDTO.Id,
                    Name = peopleDTO.Name,
                    LastName = peopleDTO.LastName,
                    Email = peopleDTO.Email
                };
            }
            return null;
        }
    }
}
