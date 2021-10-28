using System.Collections.Generic;
using Services.Freelancer.Models;

namespace Services.Freelancer.Services.Type
{
    public interface ITypeService
    {
        List<TypeModel> GetUserTypes();
    }
}