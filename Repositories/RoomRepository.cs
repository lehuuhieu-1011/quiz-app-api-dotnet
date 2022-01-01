using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_app_dotnet_api.Data;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Repositories
{
    public class RoomRepository : IRoomRepository<Room>
    {
        private readonly DataContext _context;
        public RoomRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Room> CreateRoom(Room room)
        {
            await _context.Room.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public List<Room> GetALl()
        {
            return _context.Room.Include(r => r.Course).ToList();
        }

        public Room GetById(Guid id)
        {
            return _context.Room.FirstOrDefault(x => x.Id == id);
        }
    }
}