using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        [SerializeField] private FloatingJoystick joystickInput;

        #endregion

        #region Private Variables
        [SerializeField]
        private bool _hasTouched;
        private bool _isReadyToTouch = false;
        #endregion

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onPlay += OnPlay;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onPlay -= OnPlay;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion Event Subscriptions


        private void Update()
        {
            //if (!_isReadyToTouch) return;
            JoystickInputUpdate();
        }

        private void OnReset() => _hasTouched = false;
        private void OnPlay() => _isReadyToTouch = true;

        private void JoystickInputUpdate()
        {
            if (Input.GetMouseButton(0) && !_hasTouched)
                _hasTouched = true;

            if (!_hasTouched) return;

            CharacterInputHandler();

            _hasTouched = joystickInput.Direction.sqrMagnitude > 0;
        }

        private void CharacterInputHandler()
        {
            InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
            {
                MovementVector = new Vector2(joystickInput.Horizontal, joystickInput.Vertical)
            }) ;
        }

    }
}
