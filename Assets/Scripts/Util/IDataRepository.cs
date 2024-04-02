using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    internal interface IDataRepository<T> where T : class
    {
        IEnumerable<T> GetFullData();
        T GetDataById(int id);
        void AddData(T data);
        void AddBulkData(IEnumerable<T> data);
        void UpdateData(T data);
        void DeleteDataById(int id);
        T this[int id] { get; set; }
    }
}
