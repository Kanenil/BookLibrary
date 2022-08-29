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
    public class ActionService : IService<ActionDTO>
    {
        private ActionRepository _actionRepository;
        public ActionService(string connectionString)
        {
            _actionRepository = new ActionRepository(connectionString);
        }

        public void Create(ActionDTO item)
        {
            _actionRepository.Create(ConvertActionDTOToAction(item));
        }

        public void Delete(int? id)
        {
            _actionRepository.Delete(id);
        }

        public ActionDTO Find(int? id)
        {
            return ConvertActionToActionDTO(_actionRepository.Find(id));
        }

        public IEnumerable<ActionDTO> FindAll(decimal discount)
        {
            return ConvertActionToActionDTO(_actionRepository.Find(discount));
        }

        public IEnumerable<ActionDTO> GetAll()
        {
            return ConvertActionToActionDTO(_actionRepository.GetAll());
        }

        public void Update(ActionDTO item)
        {
            _actionRepository.Update(ConvertActionDTOToAction(item));
        }

        public ActionDTO ConvertActionToActionDTO(DAL.Models.Action action)
        {
            if (action != null)
            {
                return new ActionDTO()
                {
                    Id = action.Id,
                    Start = action.Start,
                    End = action.End,
                    Discount = action.Discount
                };
            }
            return null;
        }

        public IEnumerable<ActionDTO> ConvertActionToActionDTO(IEnumerable<DAL.Models.Action> actions)
        {
            if (actions != null)
            {
                var tempActionDTO = new List<ActionDTO>();
                foreach (var item in actions)
                    tempActionDTO.Add(ConvertActionToActionDTO(item));
                return tempActionDTO;
            }
            return null;
        }

        public DAL.Models.Action ConvertActionDTOToAction(ActionDTO actionDTO)
        {
            if (actionDTO != null)
            {
                return new DAL.Models.Action()
                {
                    Id = actionDTO.Id,
                    Start = actionDTO.Start,
                    End = actionDTO.End,
                    Discount = actionDTO.Discount
                };
            }
            return null;
        }
    }
}
