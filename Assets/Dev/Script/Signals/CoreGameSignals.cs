using Controllers;
using Enums;
using Extentions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction onPlay = delegate { };
        public UnityAction onReset = delegate { };
        public UnityAction<Transform> onSetCameraTarget = delegate { };

        public UnityAction<int> onUpdateGoldScore = delegate { };

        public UnityAction onStartMoneyPayment = delegate { };
        public UnityAction onStopMoneyPayment = delegate { };

        public UnityAction<Transform> onRemoveStack = delegate { };
        public UnityAction<Stackable> onCalculateGemStackType = delegate { };

        public UnityAction<CollectedGemDatasList> onGetCollectedGemData = delegate { };

        //public UnityAction<PlayerAnimationStates> onChangePlayerAnimationState = delegate { };

    }
}
