using Assets.Scripts.Events;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Scripts.Input.Util
{
    public static class InputUtil
    {
        //public static IReadOnlyList<Type> PlayerInputs {  get; private set; }
        public static IReadOnlyList<object> InputActionAssets { get; private set; }
        public static IReadOnlyList<object> InputActionMaps { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            InputActionAssets = AssetsUtil.GetAssetsByType(typeof(InputActionAsset));
            InputActionMaps = InitializeAllMaps();
        }

        private static IReadOnlyList<object> InitializeAllMaps()
        {
            List<object> actionMaps = new List<object>();

            foreach (var ia_asset in InputActionAssets)
            {
                var actionMapsProperty = ia_asset
                    .GetType()
                    .GetProperty("actionMaps");
                var actionMapsValue = actionMapsProperty
                    .GetValue(ia_asset);
                if (actionMapsValue == null) continue;
                foreach(var map in GetElement((IEnumerable<object>)actionMapsValue))
                {
                    actionMaps.Add(map);
                }
            }

            return actionMaps;
        }

        public static IEnumerable<object> GetElement(IEnumerable<object> elements)
        {
            foreach (var element in elements)
            {
                Debug.Log($"Initialized {(element as InputActionMap).name} action map!\n" +
                    $"GUID: {(element as InputActionMap).id}");
                yield return element;
            }
        }
    }
}
