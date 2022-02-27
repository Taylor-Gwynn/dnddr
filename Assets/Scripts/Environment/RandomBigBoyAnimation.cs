using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class RandomBigBoyAnimation : MonoBehaviour
    {
        public Animator _Animator;
        public List<AnimatorController> _List;

        private void OnEnable()
        {
            SetRandomController();
        }

        private void OnDisable()
        {
            SetRandomController();
        }

        public void SetRandomController()
        {
            int rand = Random.Range(0, _List.Count);
            _Animator.runtimeAnimatorController = _List[rand];
        }
    }
}
