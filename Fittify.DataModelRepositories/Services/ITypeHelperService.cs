using System.Collections.Generic;

namespace Fittify.DataModelRepositories.Services
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields, ref List<string> errorMessages);
    }
}
