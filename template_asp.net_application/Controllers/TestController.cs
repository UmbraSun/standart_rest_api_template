﻿using BLL.Interfaces;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace template_asp.net_application.Controllers
{
    [Route("api/Test")]
    [ApiController]
    [ProducesResponseType<BadRequestDto>(StatusCodes.Status500InternalServerError)]
    public class TestController : ControllerBase
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        //[RolesAuthorize(RoleType.SuperAdmin)] 
        [ProducesResponseType<TestDto>(StatusCodes.Status200OK)]
        public async Task<TestDto> CreateTestModel(TestDto test)
        {
            return await _service.CreateTestModel(test);
        }

        [HttpGet]
        [Route("get")]
        //[RolesAuthorize(RoleType.SuperAdmin)] 
        [ProducesResponseType<TestDto>(StatusCodes.Status200OK)]
        public async Task<TestDto> GetTestModel([FromQuery] TestDto test)
        {
            return await _service.CreateTestModel(test);
        }
    }
}
