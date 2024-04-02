using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Physics.Util
{
    internal class PhysicalDataRepository : IDataRepository<PhysicalData>
    {
        private List<PhysicalData> _data = new List<PhysicalData>();

        public void AddData(PhysicalData data)
        {
            _data.Add(data);
        }
        public void AddBulkData(IEnumerable<PhysicalData> data)
        {
            _data.AddRange(data);
        }

        public void DeleteDataById(int id)
        {
            _data.RemoveAt(id);
        }

        public PhysicalData GetDataById(int id)
        {
            return _data[id];
        }

        public IEnumerable<PhysicalData> GetFullData()
        {
            return _data;
        }

        public void UpdateData(PhysicalData data)
        {
            _data[_data.IndexOf(data)] = data;
        }

        public PhysicalData this[int id] 
        {
            get
            {
                if (id < 0 ||  id >= _data.Count) throw new ArgumentOutOfRangeException("id");
                return _data[id];
            }
            set => throw new NotImplementedException(); 
        }
    }
}
