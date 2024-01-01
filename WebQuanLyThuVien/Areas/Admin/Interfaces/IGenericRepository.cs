using System.Collections.Generic;

namespace WebQuanLyThuVien.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        int Insert(T obj);
        int Update(T obj);
        int Delete(T obj);
    }
}
