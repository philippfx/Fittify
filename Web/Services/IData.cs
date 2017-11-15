using System.Collections.Generic;

namespace Web.Services
{
    public interface IData<T> where T : class
    {
        ICollection<T> GetAll();
        T Get(int id);
    }
    
}
