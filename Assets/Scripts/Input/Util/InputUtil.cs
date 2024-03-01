using Assets.Scripts.Events;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Playables;
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
            InputActionMaps = GetAllActionMaps();
            InitializeActionMaps();
        }

        private static IReadOnlyList<object> GetAllActionMaps()
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

        private static void InitializeActionMaps()
        {
            foreach(var map in InputActionMaps)
            {
                (map as InputActionMap).Enable();
            }
        }

        private static IEnumerable<object> GetElement(IEnumerable<object> elements)
        {
            foreach (var element in elements)
            {
                Debug.Log($"Initialized {(element as InputActionMap).name} action map!\n" +
                    $"GUID: {(element as InputActionMap).id}");
                yield return element;
            }
        }

        public static InputActionMap FindMap(string name)
        {
            return (InputActionMap)FindMapByObject(name);
        }
        
        public static InputActionMap FindMap(Guid id)
        {
            return (InputActionMap)FindMapByObject(id);
        }

        private static object FindMapByField(FieldInfo fieldInfo, object o)
        {
            foreach (var map in InputActionMaps)
            {
                var mapField = map
                    .GetType()
                    .GetField(fieldInfo.Name)
                    .GetValue(map);
                if (mapField == fieldInfo.GetValue(o)) return map;
            }
            return null;
        }
        
        private static object FindMapByObject(object targetValue)
        {
            var objectType = targetValue.GetType();
            foreach (var map in InputActionMaps)
            {
                var fields = map.GetType().GetFields();
                foreach (var field in fields)
                {
                    if(field.FieldType == objectType)
                    {
                        if(field.GetValue(map).Equals(targetValue)) return map;
                    }
                }

                var properties = map.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if(property.PropertyType == objectType)
                    {
                        if (property.GetValue(map).Equals(targetValue)) return map;
                    }
                }
            }
            return null;
        }
    }
}
