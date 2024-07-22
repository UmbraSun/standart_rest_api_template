using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class User : IdentityUser, IBaseModel<string>
    {
        public DateTime CreateDate { get; set ; }
        public DateTime? UpdateDate { get ; set ; }
        public DateTime? DeleteDate { get ; set; }
    }
}
