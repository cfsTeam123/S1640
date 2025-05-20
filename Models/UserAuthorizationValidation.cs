using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class UserAuthorizationValidation
    {
        public int MTransNo { get; set; }
        public int UserNo { get; set; }
        public Nullable<int> CUserNo { get; set; }
        public int ModuleNo { get; set; }
        public string AuthType { get; set; }
        public string Department { get; set; }
        public string UserType { get; set; }
        public string Dept { get; set; }
        public Nullable<int> BranchNo { get; set; }
    }
}