using System.Collections.Generic;

namespace Fittify.Services
{
    public interface IData<T> where T : class
    {
        ICollection<T> GetAll();
        T Get(int id);
    }
    
}
