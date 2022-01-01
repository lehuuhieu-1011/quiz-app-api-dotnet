using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Services;

namespace quiz_app_dotnet_api.Controllers
{
    public class RoomController : BaseApiController
    {
        private readonly RoomService _service;
        public RoomController(RoomService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateRoom(ResponseRoomModal createRoom)
        {
            Room room = new Room
            {
                Id = createRoom.Id,
                RoomId = createRoom.RoomId,
                CourseId = createRoom.CourseId,
            };
            await _service.CreateRoom(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult GetById(Guid id)
        {
            return Ok(_service.GetById(id));
        }
    }
}