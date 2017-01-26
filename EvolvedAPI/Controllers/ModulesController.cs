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

        [HttpGet]
        public IActionResult GetById([RequiredFromQuery]string id)
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
        public IActionResult GetByCoords([RequiredFromQuery]string longitude, [RequiredFromQuery]string latitude, [FromQuery]string radius)
        {
            double clongitude;
            double clatitude;
            double cradius;

            if (string.IsNullOrEmpty(radius))
            {
                radius = "0";
            }

            if (Double.TryParse(longitude, out clongitude) && Double.TryParse(latitude, out clatitude) && Double.TryParse(radius, out cradius))
            {
                return new ObjectResult(this.ModuleRepository.Get(clongitude, clatitude, cradius));
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
