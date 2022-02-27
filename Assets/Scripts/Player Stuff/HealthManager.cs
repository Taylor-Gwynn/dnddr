using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player_Stuff
{
    public class HealthManager : MonoBehaviour
    {
        [Tooltip("The rect of the actual changing hp image.")]
        public RectTransform _HpBar;
        
        [Tooltip("Natural health loss over time in % per second")]
        [Range(0,1)]
        public float _DecayRate;
        
        [Range(0,1)]
        public float _StandardHpReduction;

        [Range(0,1)]
        public float _StandardHpIncrease;

        public float _VisualTransitionSmoothing;
        
        [Range(0,1)]
        [SerializeField]
        public float _CurrentHealth;

        private float _maxWidth;
        private float _maxHeight;

        private void Awake()
        {
            _maxWidth = _HpBar.sizeDelta.x;
            _maxHeight = _HpBar.sizeDelta.y;
            _CurrentHealth = 1;
        }

        private void Update()
        {
            HealthDecay(); // apply a osu like music meter decay
            HealthClamp(); // clamp the health between the min and max range
            UpdateHealthBar(); // update the visual representation of the values
        }
        
        private void HealthDecay()
        {
            ReduceHealth(_DecayRate * Time.deltaTime);
        }

        private void HealthClamp()
        {
            _CurrentHealth = Mathf.Clamp(_CurrentHealth, 0, 1);
        }
        
        // quirky lerp
        private void UpdateHealthBar()
        {
            float newWidth = Mathf.Lerp(_HpBar.sizeDelta.x, _CurrentHealth * _maxWidth, Time.deltaTime * _VisualTransitionSmoothing);
            _HpBar.sizeDelta = new Vector2( newWidth, _maxHeight);
        }

        public void ReduceHealth(float value)
        {
            _CurrentHealth -= value;
        }

        public void ReduceHealth()
        {
            ReduceHealth(_StandardHpReduction);
        }

        public void AddHealth(float value)
        {
            _CurrentHealth += value;
        }

        public void AddHealth()
        {
            AddHealth(_StandardHpIncrease);
        }
    }
}
