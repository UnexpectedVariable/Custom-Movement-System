using System;
using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.Util
{
    public static class AssetsUtil
    {
        public static List<object> GetAssetsByType(Type assetType)
        {
            List<object> assets = new List<object>();
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets($"t:{assetType}");
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), assetType);
                if (asset == null) continue;
                assets.Add(asset);
            }
#endif
            return assets;
        }
    }
}
