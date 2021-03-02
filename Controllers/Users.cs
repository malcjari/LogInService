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
        public async Task<List<ClientUser>> GetAllUsersAsync()
        {
            List<ClientUser> returnList = new List<ClientUser>();

            var tempList = _context.Users.ToList();

            foreach (var item in tempList)
            {
                ClientUser u = new ClientUser();
                u.Id = item.Id;
                u.Name = item.Name;
                u.UserName = item.UserName;
                u.StreetNo = item.StreetNo;
                u.City = item.City;
                u.ZipCode = item.ZipCode;
                u.PhoneNumber = item.PhoneNumber;
                u.Email = item.Email;
                u.Roles = new List<string>();
                var tempUser = await _userManager.FindByNameAsync(item.UserName);
                var tempRoles =  await _userManager.GetRolesAsync(tempUser);

                foreach (var role in tempRoles)
                {
                    u.Roles.Add(role);
                }

                returnList.Add(u);
            }
            return returnList;

        }


        [HttpGet("{id}")]
        public async Task<ClientUser> GetUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            ClientUser u = new ClientUser();
            u.Id = user.Id;
            u.Name = user.Name;
            u.UserName = user.UserName;
            u.StreetNo = user.StreetNo;
            u.City = user.City;
            u.ZipCode = user.ZipCode;
            u.PhoneNumber = user.PhoneNumber;
            u.Email = user.Email;
            var tempRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in tempRoles)
            {
                u.Roles.Add(role);
            }

            

            return u;
        }

        [Route("/Login/")]
        [HttpPost]
        public async Task<LoginResultModel> PostAsync([FromBody] LoginModel loginModel)
        {
            ClientUser u = new ClientUser();
            LoginResultModel returnModel = new LoginResultModel();

            returnModel.Status = false;
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            if (user != null)
            {
                var logInResult = await _userManager.CheckPasswordAsync(user, loginModel.Password);

                if (logInResult)
                {
                    returnModel.Status = true;
                    
                    u.Id = user.Id;
                    u.Name = user.Name;
                    u.UserName = user.UserName;
                    u.StreetNo = user.StreetNo;
                    u.City = user.City;
                    u.ZipCode = user.ZipCode;
                    u.PhoneNumber = user.PhoneNumber;
                    u.Email = user.Email;
                    returnModel.ClientUser = u;


                    var tempRoles = await _userManager.GetRolesAsync(user);

                    foreach (var role in tempRoles)
                    {
                        u.Roles.Add(role);
                    }
                }
            }
            return returnModel;
        }

        public async System.Threading.Tasks.Task InitDBAsync()
        {
            User u1 = new User();
            u1.UserName = "SysAdmin";
            var res1 = await _userManager.CreateAsync(u1, "SysAdmin123!");

            User u2 = new User();
            u2.UserName = "ConfAdmin";
            var res2 = await _userManager.CreateAsync(u2, "ConfAdmin123!");

            User u3 = new User();
            u3.UserName = "TestTest";
            var res3 = await _userManager.CreateAsync(u2, "TestTest123!");
            u3.Name = "Test Testi";
            u3.PhoneNumber = "0726384625";
            u3.Email = "Test@Test.com";
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
