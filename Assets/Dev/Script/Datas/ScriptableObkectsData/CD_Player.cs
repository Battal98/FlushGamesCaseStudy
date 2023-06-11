using Datas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Scriptables
{
    [CreateAssetMenu(fileName = "CD_Player", menuName = "FlushGames/CD_Player", order = 0)]
    public class CD_Player : ScriptableObject
    {
        public PlayerData PlayerData;
    }
}
