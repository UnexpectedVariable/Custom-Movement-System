using Assets.Scripts.Util.Comparers;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Assets.Scripts.Util.JSON.Custom_Converters;

//cof = coefficient of friction

namespace Assets.Scripts.Physics.Util
{
    public static class PhysicsUtil
    {
        public enum SurfaceType
        {
            Default = 0,
            Stone,
            Wood,
            Metal,
            Rubber,
            Fabric,
            Ice
        }

        private static string _cofJson;
        public static string CoFJson
        {
            set 
            { 
                if (_cofJson == null) _cofJson = value; 
            }
        }
        public static float DefaultCof
        {
            get => 0.5f;
        }
        private static Lazy<Dictionary<(SurfaceType Surface1, SurfaceType Surface2), float>> _coFTable = new(InitializeCoFTable);

        public static Dictionary<(SurfaceType Surface1, SurfaceType Surface2), float> CoFTable
        {
            get
            {
                return _coFTable.Value;
            }
        }

        public static Dictionary<(SurfaceType Surface1, SurfaceType Surface2), float> InitializeCoFTable()
        {
            var table = JsonConvert.DeserializeObject<Dictionary<(SurfaceType, SurfaceType), float>>(_cofJson, new CoFMapConverter());
            return new Dictionary<(SurfaceType Surface1, SurfaceType Surface2), float>(table, new UnorderedPairEqualityComparer<SurfaceType>()); ;
        }

        public static float GetCoF((SurfaceType, SurfaceType) pair) 
        {
            if(_coFTable.Value.ContainsKey(pair)) return _coFTable.Value[pair];
            return DefaultCof;
        }
    }
}
