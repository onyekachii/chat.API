using chat.Domain.DTOs;
using chat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.Service.Interface
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(GroupPostRequestDTO dto, DTO baseDTO);
    }
}
