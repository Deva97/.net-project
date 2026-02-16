using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int user, string password);
    }
}
