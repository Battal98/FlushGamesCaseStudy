using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using System;

namespace Datas
{
	[Serializable]
	public class ScoreData : ISavable
	{
		public int TotalGoldScore;

        private const SaveLoadType Key = SaveLoadType.ScoreData;
        public SaveLoadType GetKey()
        {
            return Key;
        }
        public ScoreData()
        {
            
        }
    }  
}

