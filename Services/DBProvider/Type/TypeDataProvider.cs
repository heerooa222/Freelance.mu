using System.Collections.Generic;
using System.Linq;
using Services.Freelancer.Entities;
using Services.Freelancer.Utility;

namespace Services.Freelancer.DBProvider.Type
{
    public class TypeDataProvider : ITypeDataProvider
    {
        private readonly DatabaseContext _context;

        public TypeDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public List<Entities.Type> GetUserTypes()
        {
            try
            {
                return _context.Type.Where(x => !x.IdType.Equals(UserType.ADMINISTRATOR)).ToList();
            }
            catch
            {
                return new List<Entities.Type>();
            }
        }
    }
}