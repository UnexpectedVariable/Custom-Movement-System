using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.Physics.Util
{
    internal class PhysicsService : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _cofJson = null;

        private void Start()
        {
            if (_cofJson == null) throw new NullReferenceException();
            PhysicsUtil.CoFJson = _cofJson.text;
        }

        [ContextMenu("Call CoF Table")]
        private void CallTable()
        {
            Debug.Log(PhysicsUtil.CoFTable == null);
        }

        [ContextMenu("Update json")]
        private void UpdateJson()
        {
            PhysicsUtil.CoFJson = _cofJson.text;
        }

    }
}
