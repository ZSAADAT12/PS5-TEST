using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : BaseController, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<School > lstSchool = await _context.Schools.ToListAsync();
            return Ok(lstSchool);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSchool);
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                List<School> itmSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).ToListAsync();

                _context.Remove(itmSchool);

                await _context.SaveChangesAsync();

                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] School _Item, School school_Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSchool = await _context.Schools
                                                    .Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (existSchool != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Already Exist.");
                }

                _context.Schools.Add(school_Item);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Record successfully added.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] School _Item)


        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSchool = await _context.Schools
                                                    .Where(x => x.SchoolId == _Item.SchoolId)
                                                    .FirstOrDefaultAsync();

                if (existSchool == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existSchool.SchoolId = _Item.SchoolId;
                existSchool.SchoolName = _Item.SchoolName;
                existSchool.ModifiedBy = _Item.ModifiedBy;
                existSchool.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existSchool);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Record successfully updated.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        private Task Post(School item)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<School>.Post(School _Item)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> put([FromBody] School _Item)
        {
            throw new NotImplementedException();
        }
    }

    
}
