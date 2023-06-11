using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction onPlay = delegate { };
        public UnityAction onReset = delegate { };
        public UnityAction<Transform> onSetCameraTarget = delegate { };

        public UnityAction<int> onUpdateGemScore = delegate { };
        public UnityAction<int> onUpdateMoneyScore = delegate { };

        public UnityAction onStartMoneyPayment = delegate { };
        public UnityAction onStopMoneyPayment = delegate { };

    }
}
