using System.Collections.Generic;
using System.Linq;
using Services.Freelancer.DBProvider.Type;
using Services.Freelancer.Models;

namespace Services.Freelancer.Services.Type
{
    public class TypeService : ITypeService
    {
        private readonly ITypeDataProvider _typeDataProvider;

        public TypeService(ITypeDataProvider typeDataProvider)
        {
            _typeDataProvider = typeDataProvider;
        }

        public List<TypeModel> GetUserTypes()
        {
            try
            {
                var types = _typeDataProvider.GetUserTypes();
                return types.Select(x => new TypeModel
                {
                    IdType = x.IdType,
                    TypeName = x.TypeName
                }).ToList();
            }
            catch
            {
                return new List<TypeModel>();
            }
        }
    }
}