using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogInService.Models
{
    public class LoginResultModelV2
    {
        public bool Status { get; set; }
        public ClientUser ClientUser { get; set; }

        public LoginResultModelV2()
        {
            this.ClientUser = new ClientUser();
        }
    }
}
