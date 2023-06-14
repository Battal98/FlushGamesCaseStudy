using Extentions;
using UnityEngine.Events;

namespace Signals
{
	public class UISignals : MonoSingleton<UISignals>
	{
		public UnityAction<int> onUpdateGoldScoreText = delegate { };
		public UnityAction<int,GemType,int> onChangeGemScoreAndCount = delegate { };
	} 
}
