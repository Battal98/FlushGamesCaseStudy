using Keys;
using UnityEngine;
using Managers;
using Datas;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private PlayerManager manager;

        #endregion

        #region Private Variables

        private PlayerMovementData _data;

        private Vector2 _inputVector;

        private bool _isReadyToMove;

        #endregion

        #endregion
        public void SetMovementData(PlayerMovementData movementData)
        {
            _data = movementData;
        }
        public void UpdateInputValues(HorizontalInputParams inputParams)
        {
            _inputVector = inputParams.MovementVector;
            EnableMovement(_inputVector.sqrMagnitude > 0);
        }

        private void EnableMovement(bool movementStatus)
        {
            _isReadyToMove = movementStatus;
        }

        public void DisableMovement()
        {
            _isReadyToMove = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.angularDrag = 0f;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }
        private void PlayerMove()
        {
            if (_isReadyToMove)
            {
                var velocity = rigidbody.velocity;

                velocity = new Vector3(_inputVector.x * _data.PlayerSpeed,
                    Mathf.Clamp(velocity.y,
                    -9.81f,
                    0.25f),
                    _inputVector.y * _data.PlayerSpeed);
                rigidbody.velocity = velocity;
                RotatePlayer();
            }
            else if (rigidbody.velocity != Vector3.zero)
            {
                rigidbody.velocity = new Vector3(0,
                    Mathf.Clamp(rigidbody.velocity.y,
                    -9.81f,
                    0.25f),
                    0);
            }
        }
        private void RotatePlayer()
        {
            Vector3 movementDirection = new Vector3(_inputVector.x, 0, _inputVector.y);
            if (movementDirection == Vector3.zero) return;
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, toRotation, 30);
        }

    }
}