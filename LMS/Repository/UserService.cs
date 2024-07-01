using LMS.DTOs;
using LMS.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using LMS.EmailTemplates;
using LMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;



namespace LMS.Repository
{
    public class UserService : IUserService
    {

        //Create _Context Field
        private readonly DataContext _Context;
        private readonly JWTService _jwt;
        private readonly IEmailService _emailService;




        //Contructor of the UserService
        public UserService(DataContext Context, JWTService jwt, IEmailService emailService)
        {
            _Context = Context;
            _jwt = jwt;
            _emailService = emailService;
        }


        //Create User Service
        string GenerateUserId()
        {
            string currentDate = DateTime.Now.ToString("yy");
            int count = _Context.Users.Count() + 1;
            string countString = count.ToString().PadLeft(5, '0');
            return currentDate + countString;
        }
        public async Task<CreateUserResponseDto> AddUser(CreateUserRequestDto userdto, HttpContext httpContext)
        {
            var addedby = _jwt.GetUsername(httpContext);
            if (await _Context.Users.AnyAsync(e => e.Email == userdto.Email))
            {
                throw new Exception("Email Already Exists");
            }
            else
            {
                var password = "123456";


                //Pass data from dto to new user object
                var user = new User
                {
                    UserName = GenerateUserId(),
                    FName = userdto.FName,
                    LName = userdto.LName,
                    Email = userdto.Email,
                    DOB = DateOnly.Parse(userdto.DOB),
                    Address = userdto.Address,
                    PhoneNumber = userdto.PhoneNumber,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    NIC = userdto.NIC,
                    UserType = userdto.UserType,
                    AddedById = addedby,
                    Status = "free",
                };

                //Add user object to _Context
                _Context.Users.Add(user);


                //Database update
                await _Context.SaveChangesAsync();
                var htmlBody = new EmailTemplate().DefaultPassword(password);
                await _emailService.SendEmail(htmlBody, user.Email, "You are successfully registered to the library service");

                var responsedto = new CreateUserResponseDto
                {
                    UserName = user.UserName,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                };


                //Return Creating User
                return responsedto;
            }
        }
        public async Task<User> GetById(string userName)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<bool> DeleteUser(string username)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            else
            {
                _Context.Remove(user);
                await _Context.SaveChangesAsync();
                return true;
            }
        }


        public async Task<AboutUserDto> AboutUser(string username)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == username);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            else
            {
                var aboutuser = new AboutUserDto
                {
                    UserName = user.UserName,
                    FName = user.FName,
                    LName = user.LName,
                    UserType = user.UserType,
                    Email = user.Email,
                    Type = user.UserType,
                    Phone = user.PhoneNumber,
                    DOB = user.DOB,
                    Address = user.Address,
                    Status = user.Status,
                };

                return aboutuser;
            }
        }

        public async Task<bool> EditUser(EditUserRequestDto edituser, HttpContext httpContext)
        {

            var username = _jwt.GetUsername(httpContext);
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == username);
            if (user != null)
            {
                user.FName = edituser.FName;

                user.LName = edituser.LName;

                user.PhoneNumber = edituser.PhoneNumber;

                user.Address = edituser.Address;
                user.NIC = edituser.NIC;

                user.DOB = DateOnly.Parse(edituser.DOB);
                await _Context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task<List<UserListDto>> SearchUser(SearchUserDto searchUser)
        {
            var k = new List<User>(); ;
            if (searchUser.keyword == "")
            {
                k = _Context.Users.ToList();
            }
            if (searchUser.type == "all")
            {
                k = _Context.Users.Where(e =>
                   e.UserName.Contains(searchUser.keyword) ||
                   e.FName.Contains(searchUser.keyword) ||
                   e.LName.Contains(searchUser.keyword) ||
                   e.Email.Contains(searchUser.keyword) ||
                   e.Address.Contains(searchUser.keyword)
               ).ToList();
            }
            else if (searchUser.type == "username")
            {
                k = _Context.Users.Where(e => e.UserName.Contains(searchUser.keyword)).ToList();
            }
            else if (searchUser.type == "name")
            {
                k = _Context.Users.Where(e => e.FName.Contains(searchUser.keyword) || e.LName.Contains(searchUser.keyword)).ToList();
            }
            else if (searchUser.type == "email")
            {
                k = _Context.Users.Where(e => e.Email.Contains(searchUser.keyword)).ToList();
            }
            else if (searchUser.type == "address")
            {
                k = _Context.Users.Where(e => e.Address.Contains(searchUser.keyword)).ToList();
            }
            List<UserListDto> userlist = new List<UserListDto>();
            foreach (var x in k)
            {

                var user = new UserListDto
                {
                    username = x.UserName,
                    Name = x.FName + " " + x.LName,
                    Email = x.Email,
                    Role = x.UserType
                };
                userlist.Add(user);
            }
            return userlist;


        }

        public async Task<bool> ChangePassword(ChangePasswordDto request, HttpContext httpContext)

        {
            var username = _jwt.GetUsername(httpContext);
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            else
            {
                if (BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                    await _Context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Wrong Password");
                }
            }
        }

        public async Task<AboutUserDto> GetMyData(HttpContext httpContext)
        {
            var username = _jwt.GetUsername(httpContext);
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == username);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            else
            {
                var aboutuser = new AboutUserDto
                {
                    UserName = user.UserName,
                    FName = user.FName,
                    LName = user.LName,
                    UserType = user.UserType,
                    Email = user.Email,
                    Type = user.UserType,
                    Phone = user.PhoneNumber,
                    DOB = user.DOB,
                    nic = user.NIC,
                    Address = user.Address,
                    Status = user.Status,
                };

                return aboutuser;
            }
        }

        public async Task<String> GetEmail(HttpContext httpContext)
        {
            var username = _jwt.GetUsername(httpContext);
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == username);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            else
            {
                return user.Email;
            }
        }

        public async Task<bool> ChangeEmail(string newEmail, HttpContext httpContext)
        {
            var username = _jwt.GetUsername(httpContext);
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == username);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            else if (await _Context.Users.AnyAsync(e => e.Email == newEmail))
            {
                throw new Exception("Email Already Exists");
            }
            else
            {
                user.Email = newEmail;
                await _Context.SaveChangesAsync();
                return true;
            }
        }


    }
}
