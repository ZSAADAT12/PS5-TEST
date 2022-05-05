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
    public class StudentController : BaseController, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Student> lstStudent = await _context.Students.ToListAsync();
            return Ok(lstStudent);
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{StudentId}")]
        public async Task<IActionResult> Get(int SchoolId, int StudentId)
        {
            Student itmStudent = await _context.Students
                                               .Where(x => x.StudentId == StudentId && x.SchoolId == SchoolId)
                                               .FirstOrDefaultAsync();
            return Ok(itmStudent);
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{StudentId}")]
        public async Task<IActionResult> Delete(int SchoolId, int StudentId)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                Student itmStudent = await _context.Students
                                                    .Where(x => x.StudentId == StudentId && x.SchoolId == SchoolId)
                                                    .FirstOrDefaultAsync();
                _context.Remove(itmStudent);

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
        public async Task<IActionResult> Post([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students
                                                    .Where(x => x.StudentId == _Item.StudentId &&
                                                                x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (existStudent != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Already Exist.");
                }

                _context.Students.Add(_Item);
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
        public async Task<IActionResult> Put([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students
                                                    .Where(x => x.StudentId == _Item.StudentId &&
                                                                 x.SchoolId == _Item.SchoolId)
                                                    .FirstOrDefaultAsync();

                if (existStudent == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existStudent.StudentId = _Item.StudentId;
                existStudent.SchoolId = _Item.SchoolId;
                existStudent.Salutation = _Item.Salutation;
                existStudent.FirstName = _Item.FirstName;
                existStudent.LastName = _Item.LastName;
                existStudent.StreetAddress = _Item.StreetAddress;
                existStudent.Zip = _Item.Zip;
                existStudent.Phone = _Item.Phone;
                existStudent.Employer = _Item.Employer;
                existStudent.RegistrationDate = _Item.RegistrationDate;
                existStudent.ModifiedBy = _Item.ModifiedBy;
                existStudent.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existStudent);
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

        public Task<IActionResult> put([FromBody] Student _Item)
        {
            throw new NotImplementedException();
        }
    }
}
