using UnityEngine;

namespace Bim
{
    /// <summary>
    /// This class hosts interaction information for the obstacle
    /// </summary>
    [CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/NewObstacleType", order = 1)]
    public class ObstacleType : ScriptableObject
    {
        public ChoiceType _ChoiceType;
        public AnimatorOverrideController _PlayerAnimOverride;
        public AnimatorOverrideController _AnimOverride;
    }
}
