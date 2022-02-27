using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bim
{
    public class Obstacle : BeatMover
    {
        [Header("Obstacle Settings")]
        [Tooltip("The scriptable object that dictates how the player interacts with the obstacle.")]
        public ObstacleType _Type;

        public Dice_Roller _Die;

        public int _SuccessRoll = 10;
        
        public bool _IsSupposedToPass = true;   //to be set by the dice roll

        
        // invoked when the object should be returned to its pool
        protected internal readonly UnityEvent<Obstacle> Recycle = new UnityEvent<Obstacle>();

        private void Awake()
        {
            _Die.gameObject.SetActive(false);
        }

        public void ReturnObstacle()
        {
            Recycle.Invoke(this);
        }

        public void PlayFail()
        {
            
        }

        public void PlaySuccess()
        {
            
        }

        new public void Start(){
            base.Start();
            animator.runtimeAnimatorController = _Type._AnimOverride;
        }

        public ObstacleType GetObstacleType()
        {
            return _Type;
        }
        
        public override void OnBeat(){

        }
        public override void OnBar(){
            
        }
        /// <summary>
        /// Used to set the location of the Obstacle
        /// </summary>
        public void Place(Vector3 position)
        {
            transform.position = position;
        }

        //called by player, initiates interaction and sets off obstacle's  animations
        public void Interact(bool isMatch){
            animator.SetBool("successParam", isMatch == _IsSupposedToPass);

            animator.SetTrigger("WindupInteraction");
            animator.SetTrigger("BeginningAction");
            Debug.Log("overwriting controller of "+name+": _Type="+_Type+" animOverride: "+_Type._AnimOverride);
            // animator.runtimeAnimatorController = _Type._AnimOverride;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 7) return;
            _Die.gameObject.SetActive(true);
            _Die.RollDie();
            _IsSupposedToPass = _Die.GetResult() < _SuccessRoll;
        }
    }
}
