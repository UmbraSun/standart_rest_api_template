using BLL.Features.TestContext.Commands;
using BLL.Features.TestContext.Queries;
using BLL.Interfaces;
using Common;
using DTOs;
using Mediatr.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resources.Words;

namespace Mediatr.Controllers
{
    [Route("api/Mediatr/Test")]
    [ApiController]
    [ProducesResponseType<BadRequestDto>(StatusCodes.Status500InternalServerError)]
    public class TestController : ControllerBase
    {
        private ISender? _Mediatr;
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        /// <summary>
        ///     Mediatr
        /// </summary>
        protected ISender Mediator => _Mediatr ??= HttpContext.RequestServices.GetService<ISender>()!;

        [HttpPost]
        [Route("create")]
        //[RolesAuthorize(RoleType.SuperAdmin)] 
        [ProducesResponseType<IActionResult>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTestModel([FromBody] TestCreateCommand command)
        {
            throw new Exception(Resource.asdasdasdsadsad);
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("get")]
        [RolesAuthorize(RoleType.SuperAdmin)]
        [ProducesResponseType<IActionResult>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTestModel([FromQuery] TestGetQuery query)
        {
            return Ok(await Mediator.Send(query));

        }
    }
}
