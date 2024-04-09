using Assets.Scripts.MovementSystem.Player;
using Assets.Scripts.Util.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Animation.Player
{
    internal class PlayerAnimationManager : MonoBehaviour, Util.Observer.IObserver<PlayerMovementController>
    {
        [SerializeField]
        private Animator _animator = null;

        [SerializeField]
        private AnimationClip _walkingClip = null;
        [SerializeField]
        private AnimationClip _idleClip = null;

        private float _crossfadeTime = .2f;

        public void Handle(PlayerMovementController observed)
        {
            if (observed.MovementVector != Vector3.zero)
            {
                _animator.CrossFade(Animator.StringToHash(_walkingClip.name), _crossfadeTime, 0);
            }
            else
                _animator.CrossFade(Animator.StringToHash(_idleClip.name), _crossfadeTime, 0);
        }
    }
}
