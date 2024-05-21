using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Interfaces
{
    public interface IJwtToken
    {
        string GenerateToken(ApplicationUser user,string Role);
        DateTime? ExtractValidToDateFromToken(string tokenString);
    }
}
