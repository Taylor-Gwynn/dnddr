using UnityEngine;
using UnityEngine.Events;

namespace Bim
{
    public class Obstacle : BeatMover
    {
        [Header("Obstacle Settings")]
        [Tooltip("The scriptable object that dictates how the player interacts with the obstacle.")]
        public ObstacleType _Type;
        public bool _isSupposedToPass = true;   //to be set by the dice roll

        // invoked when the object should be returned to its pool
        protected internal readonly UnityEvent<Obstacle> Recycle = new UnityEvent<Obstacle>(); 

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

        //called by player, initiates interaction and sets off animations
        public void Interact(bool isMatch){
            animator.SetBool("successParam", isMatch == _isSupposedToPass);

            animator.SetTrigger("WindupInteraction");
            animator.SetTrigger("BeginningAction");
            Debug.Log("overwriting controller of "+name+": _Type="+_Type+" animOverride: "+_Type._AnimOverride);
            animator.runtimeAnimatorController = _Type._AnimOverride;
        }
    }
}
