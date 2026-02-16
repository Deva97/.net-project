using HealthApp.Domain.Entities;
using HealthApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Application.Common.Interfaces
{
    public interface IMacroSource
    {
        IMacroSource macroSource(FoodType food); 
    }


}
