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
    public class GradeTypeWeightController : BaseController, iBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lstSchool = await _context.GradeTypeWeights.ToListAsync();
            return Ok(lstSchool);
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{SectionId}/{GradeType}")]
        public async Task<IActionResult> Get(int SchoolId, int SectionId, string GradeType)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights
                                                               .Where(x => x.SchoolId == SchoolId && x.SectionId == x.SectionId
                                                                           && x.GradeTypeCode == GradeType)
                                                               .FirstOrDefaultAsync();
            return Ok(itmGradeTypeWeight);
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{SectionId}/{GradeType}")]
        public async Task<IActionResult> Delete(int SchoolId, int SectionId, string GradeType)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights
                                                                    .Where(x => x.SchoolId == SchoolId && x.SectionId == x.SectionId
                                                                                && x.GradeTypeCode == GradeType)
                                                                    .FirstOrDefaultAsync();
                _context.Remove(itmGradeTypeWeight);

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
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeights = await _context.GradeTypeWeights
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();

                if (existGradeTypeWeights != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Already Exist.");
                }

                _context.GradeTypeWeights.Add(_Item);
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
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeights = await _context.GradeTypeWeights
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.SectionId == _Item.SectionId &&
                                                                x.GradeTypeCode == _Item.GradeTypeCode
                                                    )
                                                    .FirstOrDefaultAsync();

                if (existGradeTypeWeights == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existGradeTypeWeights.SchoolId = _Item.SchoolId;
                existGradeTypeWeights.SectionId = _Item.SectionId;
                existGradeTypeWeights.GradeTypeCode = _Item.GradeTypeCode;
                existGradeTypeWeights.NumberPerSection = _Item.NumberPerSection;
                existGradeTypeWeights.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                existGradeTypeWeights.DropLowest = _Item.DropLowest;
                existGradeTypeWeights.ModifiedBy = _Item.ModifiedBy;
                existGradeTypeWeights.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existGradeTypeWeights);
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

        public Task<IActionResult> put([FromBody] GradeTypeWeight _Item)
        {
            throw new NotImplementedException();
        }
    }
}
