using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class Extensions
    {
        public static (T2 , T1) Swap<T1, T2>(this (T1, T2) pair)
        {
            return (pair.Item2,  pair.Item1);
        }

        public static Vector3 Sum(this IEnumerable<Vector3> source)
        {
            Vector3 sum = Vector3.zero;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }
    }
}
