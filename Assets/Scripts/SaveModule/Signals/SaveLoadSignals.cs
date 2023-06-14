using UnityEngine.Events;
using Extentions;
using System;
using SaveLoadModule.Enums;
using Datas;
using Controllers;

namespace SaveLoadModule.Signals
{
    public class SaveLoadSignals : MonoSingleton<SaveLoadSignals>
    {
        public UnityAction<CollectedGemDatasList, int> onSaveCollectedGemData = delegate { };
        public Func<SaveLoadType, int, CollectedGemDatasList> onLoadCollectedGemData;
        public UnityAction<ScoreData, int> onSaveScoreData = delegate { };
        public Func<SaveLoadType,int, ScoreData> onLoadScoreData;
    } 
}
