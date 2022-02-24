using UnityEngine;
using UnityEngine.Events;

namespace Bim
{
    public class Obstacle : MonoBehaviour
    {
        
        
        // invoked when the object should be returned to its pool
        protected internal readonly UnityEvent<Obstacle> Recycle = new UnityEvent<Obstacle>(); 

        private void ReturnObstacle()
        {
            Recycle.Invoke(this);
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
