using Assets.Scripts.Physics.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Physics.Util.PhysicsUtil;

namespace Assets.Scripts.Util.JSON.Custom_Converters
{
    internal class CoFMapConverter : JsonConverter<Dictionary<(SurfaceType, SurfaceType), float>>
    {
        public override Dictionary<(SurfaceType, SurfaceType), float> ReadJson(JsonReader reader, Type objectType, Dictionary<(SurfaceType, SurfaceType), float> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var table = new Dictionary<(SurfaceType, SurfaceType), float>();
            string surfacePairPattern = @"^\((.+?),(.+?)\)$";
            while (reader.Read())
            {
                if (reader.Value == null) continue;

                string raw = default;
                try
                {
                    raw = reader.Value.ToString();
                }
                catch(InvalidCastException e)
                {
                    Debug.LogError($"Coefficients of friction JSON converter was unable to cast surface pair to string");
                }

                Match match = Regex.Match(raw, surfacePairPattern);
                var surfacePair = (SurfaceType.Default, SurfaceType.Default);
                if(match.Success)
                {
                    surfacePair.Item1 = (SurfaceType)Enum.Parse(typeof(SurfaceType), match.Groups[1].Value);
                    surfacePair.Item2 = (SurfaceType)Enum.Parse(typeof(SurfaceType), match.Groups[2].Value);
                }

                if (!reader.Read())
                {
                    throw new JsonReaderException("Reader was unable to read friction coefficient");
                }

                float frictionCoefficient = PhysicsUtil.DefaultCof;
                try
                {
                    frictionCoefficient = Convert.ToSingle(reader.Value);
                }
                catch(InvalidCastException e)
                {
                    Debug.LogError($"Coefficients of friction JSON converter was unable to cast friction coefficient to float");
                }

                table.Add(surfacePair, frictionCoefficient);
            }

            return table;
        }

        public override void WriteJson(JsonWriter writer, Dictionary<(SurfaceType, SurfaceType), float> value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
