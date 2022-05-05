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
    public class InstructorController : BaseController, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstSchool = await _context.Instructors.ToListAsync();
            return Ok(lstSchool);
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{InstructorId}")]
        public async Task<IActionResult> Get(int SchoolId, int InstructorId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == SchoolId && x.InstructorId == InstructorId).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{InstructorId}")]
        public async Task<IActionResult> Delete(int SchoolId, int InstructorId)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == SchoolId && x.InstructorId == InstructorId).FirstOrDefaultAsync();

                _context.Remove(itmInstructor);

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
        public async Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInstructors = await _context.Instructors
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();

                if (existInstructors != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Already Exist.");
                }

                _context.Instructors.Add(_Item);
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
        public async Task<IActionResult> Put([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInstructors = await _context.Instructors
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.InstructorId == _Item.InstructorId)
                                                    .FirstOrDefaultAsync();

                if (existInstructors == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existInstructors.SchoolId = _Item.SchoolId;
                existInstructors.InstructorId = _Item.InstructorId;
                existInstructors.Salutation = _Item.Salutation;
                existInstructors.FirstName = _Item.FirstName;
                existInstructors.LastName = _Item.LastName;
                existInstructors.StreetAddress = _Item.StreetAddress;
                existInstructors.Zip = _Item.Zip;
                existInstructors.Phone = _Item.Phone;
                existInstructors.ModifiedBy = _Item.ModifiedBy;
                existInstructors.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existInstructors);
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
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }

        public async Task<IActionResult> Delete(int KeyValue)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }

        public Task<IActionResult> put([FromBody] Instructor _Item)
        {
            throw new NotImplementedException();
        }
    }
}
