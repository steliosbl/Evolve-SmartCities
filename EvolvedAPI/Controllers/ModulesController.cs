namespace EvolvedAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using EvolvedAPI.Models;

    [Route("api/[controller]")]
    public class ModulesController : Controller
    {
        public ModulesController(IModuleRepository moduleRepository)
        {
            this.ModuleRepository = moduleRepository;
        }

        public IModuleRepository ModuleRepository { get; private set; }

        [HttpGet]
        public IEnumerable<Module> GetAll()
        {
            return ModuleRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetModule")]
        public IActionResult GetById(string id)
        {
            int moduleID;
            if (Int32.TryParse(id, out moduleID))
            {
                var module = this.ModuleRepository.Get(moduleID);
                if (module != null)
                {
                    return new ObjectResult(module);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetByCoords([FromQuery]string longitude, [FromQuery]string latitude)
        {
            double clongitude;
            double clatitude;
            if (Double.TryParse(longitude, out clongitude) && Double.TryParse(latitude, out clatitude))
            {
                var module = this.ModuleRepository.Get(clongitude, clatitude);
                if (module != null)
                {
                    return new ObjectResult(module);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetByCoordRadius([FromQuery]string longitude, [FromQuery]string latitude, [FromQuery]string radius)
        {
            double clongitude;
            double clatitude;
            double cradius;

            if (Double.TryParse(longitude, out clongitude) && Double.TryParse(latitude, out clatitude) && Double.TryParse(radius, out cradius))
            {
                return new ObjectResult(this.ModuleRepository.Get(clongitude, clatitude, cradius);
            }
            else
            {
                return BadRequest();
            }
        }

        //public IActionResult Create([FromBody]string hub_id, [FromBody]string tlm_id, [FromBody]string state, [FromBody]string timestamp)
        //{

        //}
    }
}
