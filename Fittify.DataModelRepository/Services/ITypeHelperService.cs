using System.Collections.Generic;

namespace Fittify.DataModelRepository.Services
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields, ref List<string> errorMessages);
    }
}
