using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseController, iBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lstEnrollments = await _context.Enr.OrderBy(x => x.EnrollDate).ToListAsync();
            return Ok(lstEnrollments);
        }

        [HttpGet]
        [Route("Get/{StudentId}/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Get(int StudentId, int SectionId, int SchoolId)
        {
            Enrollment itmEnrollment = await _context.Enr
                                                     .Where(x => x.StudentId == StudentId &&
                                                                 x.SectionId == SectionId &&
                                                                 x.SchoolId == SchoolId)
                                                     .FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }

        [HttpDelete]
        [Route("Delete/{StudentId}/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Delete(int StudentId, int SectionId, int SchoolId)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                Enrollment itmEnrollment = await _context.Enr
                                                     .Where(x => x.StudentId == StudentId &&
                                                                 x.SectionId == SectionId &&
                                                                 x.SchoolId == SchoolId)
                                                     .FirstOrDefaultAsync();

                _context.Remove(itmEnrollment);

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

        public async Task<IActionResult> Post([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enr
                                                    .Where(x => x.StudentId == _Item.StudentId &&
                                                                x.SchoolId == _Item.SchoolId &&
                                                                x.SectionId == _Item.SectionId)
                                                    .FirstOrDefaultAsync();

                if (existEnrollment != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment Already Exist.");
                }

                _context.Enr.Add(_Item);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Enrollment added.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enr
                                                    .Where(x => x.StudentId == _Item.StudentId &&
                                                                x.SchoolId == _Item.SchoolId &&
                                                                x.SectionId == _Item.SectionId)
                                                    .FirstOrDefaultAsync();

                if (existEnrollment == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existEnrollment.StudentId = _Item.StudentId;
                existEnrollment.SectionId = _Item.SectionId;
                existEnrollment.EnrollDate = _Item.EnrollDate;
                existEnrollment.FinalGrade = _Item.FinalGrade;
                existEnrollment.ModifiedBy = _Item.ModifiedBy;
                existEnrollment.ModifiedDate = _Item.ModifiedDate;
                existEnrollment.SchoolId = _Item.SchoolId;

                _context.Update(existEnrollment);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Enrollment updated.");
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

        public Task<IActionResult> put([FromBody] Enrollment _Item)
        {
            throw new NotImplementedException();
        }
    }
}
