using UnityEngine;

namespace Datas.Scriptables
{
    [CreateAssetMenu(fileName = "CD_Score", menuName = "FlushGames/CD_Score", order = 0)]
    public class CD_Score : ScriptableObject
    {
        public ScoreData ScoreData;
    }
}
