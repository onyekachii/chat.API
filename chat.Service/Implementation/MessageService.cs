using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace chat.Service.Implementation
{
    public class MessageService : IMessageService
    {
        private IUnitOfWork _db;

        public MessageService(IUnitOfWork db)
        {
            _db = db;            
        }
        public async Task<List<Message>> GetMessagesByName(DateTime? last, string groupName, long appId)
        {
            var messages = await _db.Message
                .FindByCondition(m => !m.SoftDeleted && m.Group.Name == groupName && (last == null || m.CreatedDate > last) && !m.Group.SoftDeleted)
                .Include(m => m.Group)
                .ToListAsync();
            return messages;
        }
    }
}
