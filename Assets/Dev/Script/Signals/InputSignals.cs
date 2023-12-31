using Extentions;
using Keys;
using UnityEngine.Events;

namespace Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    {
        public UnityAction<HorizontalInputParams> onInputDragged = delegate { };
        public UnityAction<bool> onInputTakenActive = delegate { };
        public UnityAction onCharacterInputRelease = delegate { };
    }
}
