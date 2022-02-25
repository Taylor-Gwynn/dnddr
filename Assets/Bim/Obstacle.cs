using UnityEngine;
using UnityEngine.Events;

namespace Bim
{
    public class Obstacle : BeatMover
    {
        [Header("Obstacle Settings")]
        [Tooltip("The scriptable object that dictates how the player interacts with the obstacle.")]
        public ObstacleType _Type;

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
        
        /// <summary>
        /// Used to set the location of the Obstacle
        /// </summary>
        public void Place(Vector3 position)
        {
            transform.position = position;
        }
    }
}
