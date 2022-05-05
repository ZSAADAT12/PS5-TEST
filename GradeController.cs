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
    public class GradeController : BaseController, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lstGrades = await _context.Grades.ToListAsync();
            return Ok(lstGrades);
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{StudentId}/{SectionId}/{GradeType}/{GradeCode}")]
        public async Task<IActionResult> Get(int SchoolId, int StudentId, int SectionId, string GradeType, int GradeCode)
        {
            Grade itmGrade = await _context.Grades
                                           .Where(x => x.SchoolId == SchoolId && x.StudentId == StudentId &&
                                                       x.SectionId == SectionId && x.GradeTypeCode == GradeType &&
                                                       x.GradeCodeOccurrence == GradeCode).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{StudentId}/{SectionId}/{GradeType}/{GradeCode}")]
        public async Task<IActionResult> Delete(int SchoolId, int StudentId, int SectionId, string GradeType, int GradeCode)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                Grade itmGrade = await _context.Grades
                                                .Where(x => x.SchoolId == SchoolId && x.StudentId == StudentId &&
                                                            x.SectionId == SectionId && x.GradeTypeCode == GradeType &&
                                                            x.GradeCodeOccurrence == GradeCode).FirstOrDefaultAsync();
                _context.Remove(itmGrade);

                await _context.SaveChangesAsync();

                await trans.CommitAsync();
                return Ok("Record is successfully removed.");
            }
            catch (Exception e)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("Post")]

        public async Task<IActionResult> Post([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades
                                                    .Where(x => x.SchoolId == _Item.SchoolId && x.StudentId == _Item.StudentId &&
                                                            x.SectionId == _Item.SectionId && x.GradeTypeCode == _Item.GradeTypeCode &&
                                                            x.GradeCodeOccurrence == _Item.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (existGrade != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Already Exist.");
                }

                _context.Grades.Add(_Item);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Record is successfully added.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades
                                                    .Where(x => x.SchoolId == _Item.SchoolId && x.StudentId == _Item.StudentId &&
                                                            x.SectionId == _Item.SectionId && x.GradeTypeCode == _Item.GradeTypeCode &&
                                                            x.GradeCodeOccurrence == _Item.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (existGrade == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existGrade.SchoolId = _Item.SchoolId;
                existGrade.SectionId = _Item.SectionId;
                existGrade.StudentId = _Item.StudentId;
                existGrade.GradeTypeCode = _Item.GradeTypeCode;
                existGrade.GradeCodeOccurrence = _Item.GradeCodeOccurrence;
                existGrade.NumericGrade = _Item.NumericGrade;
                existGrade.Comments = _Item.Comments;
                existGrade.ModifiedBy = _Item.ModifiedBy;
                existGrade.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existGrade);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Record is successfully updated.");
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

        public Task<IActionResult> put([FromBody] Grade _Item)
        {
            throw new NotImplementedException();
        }
    }
}
