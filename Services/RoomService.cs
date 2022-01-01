using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Repositories;

namespace quiz_app_dotnet_api.Services
{
    public class RoomService
    {
        private readonly IRoomRepository<Room> _repo;
        public RoomService(IRoomRepository<Room> repo)
        {
            _repo = repo;
        }
        public async Task<Room> CreateRoom(Room room)
        {
            return await _repo.CreateRoom(room);
        }
        public List<Room> GetAll()
        {
            return _repo.GetALl();
        }
        public Room GetById(Guid id)
        {
            return _repo.GetById(id);
        }
    }
}