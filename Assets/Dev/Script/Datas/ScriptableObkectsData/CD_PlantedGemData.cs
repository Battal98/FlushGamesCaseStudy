using System.Collections.Generic;
using UnityEngine;

namespace Datas.Scriptables
{
    [CreateAssetMenu(fileName = "CD_PlantedGemData", menuName = "FlushGames/CD_PlantedGemData", order = 0)]
    public class CD_PlantedGemData : ScriptableObject
    {
        public List<PlantedGemsData> PlantedGemsDatas;
    }
}