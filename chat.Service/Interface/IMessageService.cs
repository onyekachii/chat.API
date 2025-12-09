using chat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service.Interface
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessagesByName(DateTime? lastCreated, string groupName, long appId);
    }
}
