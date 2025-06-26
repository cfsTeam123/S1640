using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class UserValidation
    {
        public int MTransNo { get; set; }
        [Required(ErrorMessage = "Please Enter The UserName")]
        public string UserName { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^[0-9a-zA-Z''-''.''@'','':'';''&''('')''%''#''*'\s]{1,250}$", ErrorMessage = "Special Characters are not allowed.")]
        [Required(ErrorMessage = "Please Enter The Login ID")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please Enter The Password")]
        public string PW { get; set; }
        public string RePW { get; set; }
        [Required(ErrorMessage = "Please Select the UserType")]
        public string UserType { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public Nullable<byte> PWExpiry { get; set; }
        public Nullable<byte> BadLogins { get; set; }
        public Nullable<byte> LockCounter { get; set; }
        [Required(ErrorMessage = "Please Select the LockStatus")]
        public string LockStatus { get; set; }
        public Nullable<short> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<short> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string DeleteStatus { get; set; }
        public Nullable<short> SubscID { get; set; }
        public Nullable<System.DateTime> loginDate { get; set; }
        public string Department { get; set; }
        public Nullable<int> BranchNo { get; set; }
        public bool RememberMe { get; set; }

        public string UserMode { get; set; }
    }
}