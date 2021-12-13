using AutoMapper;
using System.Linq;
using System.Net;
using System.Web.Http;
using Users.Application.Models;
using Users.Application.Repositories;
using Users.Application.Services;
using Users.Core.Entities;

namespace MiniStore.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login(UserLoginDto userCreateDto)
        {
            var user = mapper.Map<User>(userCreateDto);

            var repoUser = unitOfWork.Users.Get(u => u.Email.Equals(user.Email)).FirstOrDefault();

            if (repoUser == null)
                NotFound();

            if (repoUser.Password != user.Password)
                return StatusCode(HttpStatusCode.Forbidden);

            return Ok(TokenManager.GenerateToken(user.Email));
        }

        [HttpGet]
        public IHttpActionResult Validate(string token, string email)
        {
            if (!unitOfWork.Users.Get(u => u.Email.Equals(email)).Any())
                return NotFound();

            if (TokenManager.ValidateToken(token)?.Equals(email) ?? false)
                return Ok();

            return BadRequest();
        }
    }
}