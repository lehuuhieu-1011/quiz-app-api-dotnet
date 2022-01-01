using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Repositories
{
    public interface IRoomRepository<T>
    {
        List<Room> GetALl();
        Room GetById(Guid id);
        Task<Room> CreateRoom(Room room);
    }
}