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
    public class GraqdeTypeController : BaseController, iBaseController<GradeType>
    {


        public GraqdeTypeController(SWARMOracleContext context,
                                 IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)

        {

        }

        public Task<IActionResult> Post([FromBody] GradeType _Item)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> put([FromBody] GradeType _Item)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<GradeType>.Delete(int keyvalue)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<GradeType>.Get()
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<GradeType>.Get(int keyvalue)
        {
            throw new NotImplementedException();
        }
    }
}
