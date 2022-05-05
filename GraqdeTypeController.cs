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
    public class GradeTypeController : BaseController, iBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lstSchool = await _context.GradeTypes.ToListAsync();
            return Ok(lstSchool);
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{GradeType}")]
        public async Task<IActionResult> Get(int SchoolId, string GradeType)
        {
            GradeType itmGradeType = await _context.GradeTypes
                                                   .Where(x => x.SchoolId == SchoolId && x.GradeTypeCode == GradeType)
                                                   .FirstOrDefaultAsync();
            return Ok(itmGradeType);
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{GradeType}")]
        public async Task<IActionResult> Delete(int SchoolId, string GradeType)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                GradeType itmGradeType = await _context.GradeTypes
                                                        .Where(x => x.SchoolId == SchoolId && x.GradeTypeCode == GradeType)
                                                        .FirstOrDefaultAsync();
                _context.Remove(itmGradeType);

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
        public async Task<IActionResult> Post([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();

                if (existGradeType != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Already Exist.");
                }

                _context.GradeTypes.Add(_Item);
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
        public async Task<IActionResult> Put([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes
                                                    .Where(x => x.SchoolId == _Item.SchoolId &&
                                                                x.GradeTypeCode == _Item.GradeTypeCode)
                                                    .FirstOrDefaultAsync();

                if (existGradeType == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existGradeType.SchoolId = _Item.SchoolId;
                existGradeType.GradeTypeCode = _Item.GradeTypeCode;
                existGradeType.Description = _Item.Description;
                existGradeType.ModifiedBy = _Item.ModifiedBy;
                existGradeType.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existGradeType);
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

        public Task<IActionResult> put([FromBody] GradeType _Item)
        {
            throw new NotImplementedException();
        }
    }
}
