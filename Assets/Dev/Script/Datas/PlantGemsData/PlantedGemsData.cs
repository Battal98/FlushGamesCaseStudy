using System;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public class PlantedGemsData
    {
        public GemType GemType;
        public float GrownTime;
        public float CollectibleGemScale;
        public GameObject SpawnObject;
        public int SalePrice;
        public Sprite Icon;
    } 
}
