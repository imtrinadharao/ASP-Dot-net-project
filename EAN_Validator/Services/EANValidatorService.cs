using EANValidator.Data;
using EANValidator.Models;
using Microsoft.EntityFrameworkCore;

namespace EANValidator.Services
{
    public class EANValidatorService
    {
        private readonly ApplicationDbContext _context;

        public EANValidatorService(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool IsValidEAN(string ean)
        {
            return _context.Products.Any(p => p.EAN == ean);
        }

        
        public Product GetEANDetails(string ean)
        {
            return _context.Products.FirstOrDefault(p => p.EAN == ean);
        }
    }
}
