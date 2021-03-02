using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogInService.Models
{
    public class LoginResultModel
    {
        public bool Status { get; set; }

        public ClientUser ClientUser { get; set; }

        public LoginResultModel()
        {
            this.ClientUser = new ClientUser();
        }
    }
}
