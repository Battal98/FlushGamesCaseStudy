using Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[CreateAssetMenu(fileName = "CD_CollectedGemData", menuName = "FlushGames/CD_CollectedGemData", order = 0)]
	public class CD_CollectedGemData : ScriptableObject
	{
		public List<CollectedGemDatas> CollectedGemData = new List<CollectedGemDatas>();
	} 
}