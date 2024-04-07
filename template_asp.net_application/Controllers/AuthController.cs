using DTOs;
using Microsoft.AspNetCore.Mvc;
using template_asp.net_application.Services;

namespace template_asp.net_application.Controllers
{
    [Route("api/Token")]
    [ApiController]
    [ProducesResponseType<BadRequestDto>(StatusCodes.Status500InternalServerError)]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get access and refresh token by login and password
        /// </summary>
        /// <param name="model">login and password</param>
        [HttpPost]
        [Route("Access")]
        [ProducesResponseType<Auth.Responce>(StatusCodes.Status200OK)]
        public async Task<Auth.Responce> AccessToken(Auth.Login model)
        {
            return await _service.AccessToken(model);
        }

        /// <summary>
        /// Get new refresh token by old token
        /// </summary>
        /// <param name="model">old token</param>
        [HttpPost]
        [Route("Refresh")]
        [ProducesResponseType<Auth.Responce>(StatusCodes.Status200OK)]
        public async Task<Auth.Responce> RefreshToken(Auth.Refresh model)
        {
            return await _service.RefreshToken(model);
        }
    }
}
