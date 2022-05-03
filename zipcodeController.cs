using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SWARM.Server.Controllers
{
    public class zipcodeController : BaseController, iBaseController<Zipcode>
    {
        public zipcodeController(SWARMOracleContext context,
                                IHttpContextAccessor httpContextAccessor)
           : base(context, httpContextAccessor)

        {

        }

        public Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> put([FromBody] Zipcode _Item)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<Zipcode>.Delete(int keyvalue)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<Zipcode>.Get()
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<Zipcode>.Get(int keyvalue)
        {
            throw new NotImplementedException();
        }
    }
}
