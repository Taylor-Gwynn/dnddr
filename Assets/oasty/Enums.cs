public enum ChoiceType
{
    Dex,
    Str,
    Cha,
    Int,
    Con,
    None
}

//a unique identifier for each obstacle (for identifying animations)
public enum ObstacleID
{
    Mountain,
    Enemy,
    Girl,
    Door,
    Ballista
}

// different input timings / scores
public enum Judgement
{
    miss,
    bad,
    goodEarly,
    goodLate,
    greatEarly,
    greatLate,
    perfectEarly,
    perfectLate
}

// public enum PlayerSuccessAnimations
// {
//     Attack,
//     Kiss,
//     Hug,
//     Jump
// }

// public enum PlayerFailAnimations
// {
//     SwingAndMiss,
//     RejectedKiss,
//     RejectedHug,
//     Trip
// }