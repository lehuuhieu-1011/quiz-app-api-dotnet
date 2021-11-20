using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace quiz_app_dotnet_api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [EnableCors("AllowAll")]
    public class BaseApiController : ControllerBase
    {
    }
}