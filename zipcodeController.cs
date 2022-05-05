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
    public class ZipcodeController : BaseController, iBaseController<Zipcode>
    {
        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZipcode = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lstZipcode);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Zipcode itmCourse = await _context.Zipcodes.Where(x => x.Zip == KeyValue.ToString()).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                Zipcode itmCourse = await _context.Zipcodes.Where(x => x.Zip == KeyValue.ToString()).FirstOrDefaultAsync();
                _context.Remove(itmCourse);
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

        public async Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZip = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (existZip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Zipcode Already Exist.");
                }
                Zipcode _zipcode = new();
                _zipcode.Zip = _Item.Zip;
                _zipcode.City = _Item.City;
                _zipcode.State = _Item.State;

                _context.Zipcodes.Add(_Item);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZip = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (existZip == null)
                {
                    await Post(_Item);
                    return Ok();
                }

                existZip.City = _Item.City;
                existZip.State = _Item.State;
                existZip.ModifiedBy = _Item.ModifiedBy;
                existZip.ModifiedDate = _Item.ModifiedDate;

                _context.Update(existZip);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public Task<IActionResult> put([FromBody] Zipcode _Item)
        {
            throw new NotImplementedException();
        }
    }
}
