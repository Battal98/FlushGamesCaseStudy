using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [System.Serializable]
    public class PlayerData
    {
        public PlayerMovementData PlayerMovementData;
    } 


    [System.Serializable]
    public class PlayerMovementData
    {
        public float PlayerSpeed;
    }
}
