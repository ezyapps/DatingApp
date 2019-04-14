using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository datingRepo, IMapper mapper)
        {
            _mapper = mapper;
            _datingRepo = datingRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _datingRepo.GetUsers();
            var usersForSend = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersForSend);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepo.GetUser(id);
            if (user == null)
                return BadRequest("Sorry! No user found.");
            var userToSend = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToSend);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForDetailedDto){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            var userFromRepo = await _datingRepo.GetUser(id);
            _mapper.Map(userForDetailedDto, userFromRepo);
            if(await _datingRepo.SaveAll())
                return NoContent();
            throw new Exception($"Updating user {id} failed on saving.");
        }

    }
}