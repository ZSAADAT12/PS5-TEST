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

namespace SWARM.Server.Controllers.Application
{
    public class InstructorController : BaseController, iBaseController<Instructor>
    {


        public InstructorController(SWARMOracleContext context,
                                 IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)

        {

        }


        Task<IActionResult> iBaseController<Instructor>.Get()
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<Instructor>.Get(int keyvalue)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> put([FromBody] Instructor _Item)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<Instructor>.Delete(int keyvalue)
        {
            throw new NotImplementedException();
        }

    }
}
