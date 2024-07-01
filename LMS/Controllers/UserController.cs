using LMS.DTOs;
using LMS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        //Create IUserService Field
        private readonly IUserService _userService;

        //Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddUser")]
        public async Task<CreateUserResponseDto> AddUser(CreateUserRequestDto userdto)
        {
            var x = HttpContext;
            return await _userService.AddUser(userdto, x);
        }
        [HttpPut("EditUser")]
        public async Task<bool> EditUser(EditUserRequestDto edituser)
        {
            var x = HttpContext;
            return await _userService.EditUser(edituser, x);
        }

        [HttpGet("DeleteUser")]
        public async Task<bool> DeleteUser(string username)
        {
            return await _userService.DeleteUser(username);
        }


        [HttpGet("AboutUser")]
        public async Task<AboutUserDto> AboutUser(string username)
        {
            return await _userService.AboutUser(username);
        }

        [HttpPost("SearchUser")]
        public async Task<List<UserListDto>> SearchUsers(SearchUserDto searchUser)
        {

            return await _userService.SearchUser(searchUser);
        }

        [HttpPut("ChangePassword")]
        public async Task<bool> ChangePassword(ChangePasswordDto request)
        {
            var x = HttpContext;
            return await _userService.ChangePassword(request, x);
        }

        [HttpGet("GetMyData")]
        public async Task<AboutUserDto> GetMyData()
        {
            var x = HttpContext;
            return await _userService.GetMyData(x);
        }

        [HttpGet("GetEmail")]
        public async Task<String> GetEmail()
        {
            var x = HttpContext;
            return await _userService.GetEmail(x);
        }

        [HttpPut("ChangeEmail")]
        public async Task<bool> ChangeEmail(string newEmail)
        {
            var x = HttpContext;

            return await _userService.ChangeEmail(newEmail, x);
        }
    }
}

