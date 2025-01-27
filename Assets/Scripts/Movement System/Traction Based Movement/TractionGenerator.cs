﻿using Assets.Scripts.MovementSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Movement_System.Traction_Based
{
    public class TractionGenerator : MonoBehaviour, Util.Observer.IObserver<SupportCollidersTracker>
    {
        private bool _canGenerateForce = default;
        //private float _forceOutput = 1f;

        public bool CanGenerateForce
        {
            get => _canGenerateForce;
        }

        public void Handle(SupportCollidersTracker observed)
        {
            var supportCount = observed.SupportColliders.Count;
            if (_canGenerateForce)
            {
                if (supportCount == 0) _canGenerateForce = false;
            }
            else
            {
                if (supportCount > 0) _canGenerateForce = true;
            }
        }
    }
}

//each surface type pair has its own coefficient of friction - prototype done
//when foot touches the surface it exerts vertical force that depends on mass and is roughly equal to its weight
//when moving vertical force raises in proportion to work done, i.e. raises slightly when walking and more when running(force = work/distance|mass*acceleration)
//by default foot has static friction force(i.e. it does not move relative to the surface it exerts force against)
//if tangential force overcomes the product of coefficient of friction and vertical force, leg's static friction becomes kinetic(i.e. it slips)
//tangential force - is the force acting parallel to the walking surface, propelling you forward as you walk
//when friction becomes kinetic, foot is doing work(i.e. moving over distance) that subtracts from the work done by leg overall
//meaning object moves slower with the same amount of power spent