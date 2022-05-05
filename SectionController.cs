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
    public class SectionController : BaseController, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Section> lstSection = await _context.Section.ToListAsync();
            return Ok(lstSection);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }

        [HttpGet]
        [Route("GetSection/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> GetSection(int SchoolId, int SectionId)
        {
            Section itmSection = await _context.Section
                                               .Where(x => x.SectionId == SectionId
                                                        && x.SchoolId == SchoolId)
                                               .FirstOrDefaultAsync();
            return Ok(itmSection);
        }



        [HttpDelete]
        [Route("DeleteSection/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Delete(int SchoolId, int SectionId)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                List<Section> itmSection = await _context.Section
                                                         .Where(x => x.SectionId == SectionId && x.SchoolId == SchoolId)
                                                         .ToListAsync();

                _context.Remove(itmSection);

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
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Section
                                                 .Where(x => x.SectionId == _Item.SectionId && x.SchoolId == _Item.SchoolId)
                                                 .FirstOrDefaultAsync();

                if (existSection != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Section already exist.");
                }

                _context.Section.Add(_Item);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Section added.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Section
                                                    .Where(x => x.SectionId == _Item.SectionId &&
                                                                 x.SchoolId == _Item.SchoolId)
                                                    .FirstOrDefaultAsync();

                if (existSection == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                existSection.SectionId = _Item.SectionId;
                existSection.SchoolId = _Item.SchoolId;
                existSection.CourseNo = _Item.CourseNo;
                existSection.SectionNo = _Item.SectionNo;
                existSection.StartDateTime = _Item.StartDateTime;
                existSection.Location = _Item.Location;
                existSection.InstructorId = _Item.InstructorId;
                existSection.Capacity = _Item.Capacity;
                existSection.ModifiedBy = _Item.ModifiedBy;
                existSection.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existSection);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Section updated.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        Task<IActionResult> iBaseController<Section>.Delete(int keyvalue)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> iBaseController<Section>.put(Section _Item)
        {
            throw new NotImplementedException();
        }
    }
}
