using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Physics.Util.PhysicsUtil;

namespace Assets.Scripts.Physics
{
    public class Surface : MonoBehaviour
    {
        [SerializeField]
        private SurfaceType _type = default;

        public SurfaceType Type 
        { 
            get => _type;
        }
    }
}
