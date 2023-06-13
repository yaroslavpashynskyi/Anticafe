using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Halls;
using BLL.Services.Halls;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class HallsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<HallDto>>> GetHall()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HallDto>> GetHall(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateHall(Hall hall)
        {
            return Ok(await Mediator.Send(new Create.Command { Hall = hall }));
        }
        [HttpPost("{id:Guid}/{id1:Guid}")]
        public async Task<IActionResult> CreateActivity(Guid idHall, Guid idActivity)
        {
            return Ok(await Mediator.Send(new AddActivity.Command { HallId = idHall, ActivityId = idActivity }));
        }
        [HttpPost("turnkey/{id:Guid}")]
        public async Task<IActionResult> Turnkey(Guid id)
        {
            return Ok(await Mediator.Send(new Turnkey.Command { Id = id }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, Hall hall)
        {
            hall.Id = id;
            return Ok(await Mediator.Send(new Edit.Command { Hall = hall }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}