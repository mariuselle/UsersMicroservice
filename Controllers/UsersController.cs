using AutoMapper;
using FluentValidation;
using System;
using System.Linq;
using System.Web.Http;
using Users.Application.AsyncServices;
using Users.Application.Models;
using Users.Application.Repositories;
using Users.Core.Entities;

namespace MiniStore.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<User> validator;
        private readonly IMessageBusClient messageBus;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<User> validator, IMessageBusClient messageBus)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.validator = validator;
            this.messageBus = messageBus;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsers()
        {
            var users = unitOfWork.Users.Get().ToList();

            return Ok(users);
        }

        [HttpGet]
        [Route("{userId}", Name = "GetUserById")]
        public IHttpActionResult GetUserById(int userId)
        {
            var user = unitOfWork.Users.GetByID(userId);
            if (user == null)
                return NotFound();

            return Ok(mapper.Map<UserReadDto>(user));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddUser(UserCreateDto userCreateDto)
        {
            User user = mapper.Map<User>(userCreateDto);

            try
            {
                validator.ValidateAndThrow(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (unitOfWork.Users.Get(u => u.Email.Equals(user.Email)).Any())
                return BadRequest("User already exists!");

            unitOfWork.Users.Insert(user);
            unitOfWork.Commit();

            var userReadDto = mapper.Map<UserReadDto>(user);

            messageBus.Publish(new Message()
            {
                Data = userReadDto,
                Type = MessageType.ADD_USER
            });

            return CreatedAtRoute(nameof(GetUserById), new { userId = user.Id }, userReadDto);
        }
    }
}