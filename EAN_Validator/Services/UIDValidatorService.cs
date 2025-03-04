using EANValidator.Data; 
using EANValidator.Models;
using Microsoft.EntityFrameworkCore;

namespace EANValidator.Services
{
    public class UIDValidatorService
    {
        //private readonly ApplicationDbContext _context;

        //public UIDValidatorService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //// Validate if the UID exists in the Nitem table
        //public bool IsValidUID(string uid)
        //{
        //    return _context.Nitems.Any(n => n.UID == uid); 
        //}

        //// Get the details of the Nitem based on UID
        //public Nitem GetUIDDetails(string uid)
        //{
        //    return _context.Nitems.FirstOrDefault(n => n.UID == uid);  
        //}

        private readonly ApplicationDbContext _context;

        public UIDValidatorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsUIDExists(string uid)
        {
            return _context.Nitems.Any(n => n.UID == uid); 
        }
    }
}
