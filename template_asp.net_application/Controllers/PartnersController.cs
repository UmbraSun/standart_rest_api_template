using BLL.Interfaces;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace template_asp.net_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType<BadRequestDto>(StatusCodes.Status500InternalServerError)]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnersService _partnersService;

        public PartnersController(IPartnersService partnersService)
        {
            _partnersService = partnersService;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        public async Task<int> CreatePartner(PartnersDto.Add dto)
        {
            return await _partnersService.Create(dto);
        }
    }
}
