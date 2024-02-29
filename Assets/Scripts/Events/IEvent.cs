using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    public interface IEvent
    {
    }

    public class MovementEvent : IEvent
    {
        public UnityEngine.Vector3 direction;
    }
}
