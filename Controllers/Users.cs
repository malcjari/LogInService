using LogInService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<Users> _logger;

        public Users(UserManager<User> userManager,
            SignInManager<User> signInManager,
            MyContext context,
            RoleManager<Role> roleManager,
            ILogger<Users> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;

            

        }



        [HttpGet]
        public async Task<List<ClientUser>> GetAllUsersAsync()
        {
            List<ClientUser> returnList = new List<ClientUser>();
            try
            {
                

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
                    var tempRoles = await _userManager.GetRolesAsync(tempUser);

                    foreach (var role in tempRoles)
                    {
                        u.Roles.Add(role);
                    }

                    returnList.Add(u);
                    return returnList;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                
            }
            
            return returnList;

        }


        [HttpGet("{id}")]
        public async Task<ClientUser> GetUserAsync(int id)
        {
            

            ClientUser u = new ClientUser();
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
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
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }
            

            

            return u;
        }

        [Route("/Login/")]
        [HttpPost]
        public async Task<LoginResultModel> PostAsync([FromBody] LoginModel loginModel)
        {
            ClientUser u = new ClientUser();
            LoginResultModel returnModel = new LoginResultModel();

            try
            {
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


                        var tempRoles = await _userManager.GetRolesAsync(user);

                        foreach (var role in tempRoles)
                        {
                            u.Roles.Add(role);
                        }
                    }
                }
                return returnModel;
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }
            
            return returnModel;
        }

    }
}
