using LogInService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogInService.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly MyContext _context;

        public Users(UserManager<User> userManager,
            SignInManager<User> signInManager,
            MyContext context,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        [HttpGet]
        public List<User> GetAllUsersAsync()
        {
            var returnList = _context.Users.ToList();
            return returnList;

        }


        [HttpGet("{id}")]
        public async Task<User> GetUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return user;
        }

        [Route("/Login/")]
        [HttpPost]
        public async Task<LoginResultModel> PostAsync([FromBody] LoginModel loginModel)
        {

            LoginResultModel returnModel = new LoginResultModel();

            returnModel.Status = false;
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            if (user != null)
            {
                var logInResult = await _userManager.CheckPasswordAsync(user, loginModel.Password);

                if (logInResult)
                {
                    returnModel.Status = true;
                    var list = await _userManager.GetRolesAsync(user);

                    foreach (var item in list)
                    {
                        returnModel.Role.Add(item);
                    }
                }
            }
            return returnModel;
        }



        //[Route("Create/")]
        //[HttpPost]
        //public async Task<string> CreateUser([FromBody] User user)
        //{

        //    string response = "Failed";

        //    var userExists = await _userManager.FindByNameAsync(user.UserName);

        //    if (userExists != null)
        //    {
        //        response = "User Already Exists!";

        //    } else
        //    {
        //        var createResult = await _userManager.CreateAsync(user, user.PasswordHash);

        //        if (createResult.Succeeded)
        //        {
        //            response = "User Succesfully Created!";
        //        }

        //    }


        //    return response;
        //}

        //[Route("/Reset/{id}")]
        //[HttpPost]
        //public async Task<string> ResetPassword(int id,[FromBody] string password)
        //{

        //    string response = "Failed";

        //    var user = await _userManager.FindByIdAsync(id.ToString());

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //    var result = await _userManager.ResetPasswordAsync(user, token, password);

        //    if (result.Succeeded)
        //    {
        //        response = "Password Successfully Changed";
        //    }


        //    return response;
        //}


        //[HttpPut]
        //public async Task<string> UpdateUser([FromBody]User user)
        //{
        //    string response = "Failed";
        //    User newUser = await _userManager.FindByIdAsync(user.Id.ToString());


        //    newUser.Name = user.Name;
        //    newUser.UserName = user.UserName;
        //    newUser.Email = user.Email;
        //    newUser.City = user.City;
        //    newUser.StreetNo = user.StreetNo;
        //    newUser.ZipCode = user.ZipCode;



        //    var result = _userManager.UpdateAsync(newUser);
        //    if (result.Result.Succeeded)
        //    {
        //        response = "User Successfully Updated";
        //    }
        //    _context.SaveChanges();

        //    return response;

        //}


        //[HttpDelete("{id}")]
        //public async Task<string> Delete(int id)
        //{
        //    string response = "Failed";

        //    var user = await _userManager.FindByIdAsync(id.ToString());

        //    var result = await _userManager.DeleteAsync(user);

        //    if (result.Succeeded)
        //    {
        //        response = "User Successfully Deleted";
        //    }


        //    return response;
        //}
    }
}
