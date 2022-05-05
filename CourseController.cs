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
    public class CourseController : BaseController, iBaseController<Course>
    {


        public CourseController(SWARMOracleContext context,
                                 IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)

        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{courseNo}")]
        public async Task<IActionResult> Get(int pSchoolId, int pCourseNo)
        {
            try
            {
                Course itmCourse = await _context.Courses
                   .Where(x => x.SchoolId == SchoolId &&
                               x.CourseNo == CourseNo)
                       .FirstOrDefaultAsync();
                return Ok(itmCourse);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return null;
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{CourseNo}")]
        public async Task<IActionResult> Delete(int SchoolId, int CourseNo)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {


                Course itmCourse = await _context.Courses
                   .Where(x => x.SchoolId == SchoolId &&
                               x.CourseNo == CourseNo)
                       .FirstOrDefaultAsync();
                _context.Remove(itmCourse);
                await _context.SaveChangesAsync();

                await trans.CommitAsync();

                return Ok("Record successfully removed.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Item)
        {

            try
            {
                var _crse = await _context.Courses
                    .Where(x => x.SchoolId == SchoolId &&
                               x.CourseNo == CourseNo)
                       .FirstOrDefaultAsync();
                if (_crse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Course already exist!");

                }




                _crse = new Course();
                _crse.Cost = _Item.Cost;
                _crse.Description = _Item.Description;
                _crse.Prerequisite = _Item.Prerequisite;
                _crse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                _crse.SchoolId = _Item.SchoolId;

                _context.Courses.Add(_crse);
                await _context.SaveChangesAsync();


                return Ok(_Item.CourseNo);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        [HttpPut]
        public async Task<IActionResult> put([FromBody] Course _Item)
        {

            try
            {
                var _crse = await_context.Courses
                    .Where(x => x.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();

                if (_crse == null)
                {
                    await Post(_Item);
                    return Ok();
                }
                _crse.Cost = _Item.Cost;
                _crse.Description = _Item.Description;
                _crse.Prerequisite = _Item.Prerequisite;
                _crse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                _crse.SchoolId = _Item.SchoolId;

                _context.Courses.Update(_crse);
                await _context.SaveChangesAsync();


                return Ok(_Item.CourseNo);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public Task<IActionResult> Delete(int keyvalue)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(int keyvalue)
        {
            throw new NotImplementedException();
        }
    }

    internal class await_context
    {
        public static IEnumerable<object> Courses { get; internal set; }
    }
}
