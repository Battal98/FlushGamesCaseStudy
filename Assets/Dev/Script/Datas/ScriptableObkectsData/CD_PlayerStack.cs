using System.Collections.Generic;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_PlayerStack", menuName = "FlushGames/CD_PlayerStack", order = 0)]
    public class CD_PlayerStack : ScriptableObject
    {
        public PlayerStackData PlayerStackData;
    }
}