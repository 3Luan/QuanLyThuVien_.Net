using System.Collections.Generic;

namespace WebQuanLyThuVien.Interfaces
{
    /// <summary>
    ///  khai báo chức năng cơ bản của entity nào cũng có
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(int obj);
        void Save();
    }
}