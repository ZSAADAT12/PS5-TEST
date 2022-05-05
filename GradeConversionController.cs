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
    public class GradeConversionController : BaseController, iBaseController<GradeConversion>
    {
        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lstSchool = await _context.GradeConversions.ToListAsync();
            return Ok(lstSchool);
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Get(int SchoolId, string LetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions
                                                               .Where(x => x.SchoolId == SchoolId && x.LetterGrade == LetterGrade)
                                                               .FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Delete(int SchoolId, string LetterGrade)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                GradeConversion itmGradeConversion = await _context.GradeConversions
                                                                    .Where(x => x.SchoolId == SchoolId && x.LetterGrade == LetterGrade)
                                                                    .FirstOrDefaultAsync();
                _context.Remove(itmGradeConversion);

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
        public async Task<IActionResult> Post([FromBody] GradeConversion _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConversion = await _context.GradeConversions
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.LetterGrade == _Item.LetterGrade).FirstOrDefaultAsync();

                if (existGradeConversion != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Already Exist.");
                }

                _context.GradeConversions.Add(_Item);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("School added.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] GradeConversion _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConversion = await _context.GradeConversions
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.LetterGrade == _Item.LetterGrade
                                                    )
                                                    .FirstOrDefaultAsync();

                if (existGradeConversion == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existGradeConversion.SchoolId = _Item.SchoolId;
                existGradeConversion.LetterGrade = _Item.LetterGrade;
                existGradeConversion.GradePoint = _Item.GradePoint;
                existGradeConversion.MaxGrade = _Item.MaxGrade;
                existGradeConversion.MinGrade = _Item.MinGrade;
                existGradeConversion.ModifiedBy = _Item.ModifiedBy;
                existGradeConversion.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existGradeConversion);
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

        public Task<IActionResult> put([FromBody] GradeConversion _Item)
        {
            throw new NotImplementedException();
        }
    }
}
