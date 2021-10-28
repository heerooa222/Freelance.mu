using System.Collections.Generic;

namespace Services.Freelancer.DBProvider.Type
{
    public interface ITypeDataProvider
    {
        List<Entities.Type> GetUserTypes();
    }
}