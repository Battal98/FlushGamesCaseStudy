using Controllers;
using Datas.Scriptables;
using Datas;
using Enums;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField]
        private PlayerAnimationController animationController;

        [SerializeField]
        private PlayerMovementController movementController;

        #endregion

        #region Private Variables

        [ShowInInspector]
        private PlayerData _data;
        private const string DataPath = "Data/CD_Player";

        #endregion

        #endregion

        #region Initialize

        private void Awake()
        {
            _data = GetPlayerData();
            Init();
            CoreGameSignals.Instance.onSetCameraTarget(transform);
        }
        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(DataPath).PlayerData;
        private void Init() => SetDataToControllers();
        private void SetDataToControllers()
        {
            movementController.SetMovementData(_data.PlayerMovementData);
        } 

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputDragged += OnGetInputValues;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputDragged -= OnGetInputValues;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void OnGetInputValues(HorizontalInputParams inputParams)
        {
            movementController.UpdateInputValues(inputParams);
            PlayerAnimationStates playerAnimationStates;
            playerAnimationStates = inputParams.MovementVector.sqrMagnitude > 0 ? PlayerAnimationStates.Run : PlayerAnimationStates.Idle;
            animationController.PlayAnimation(playerAnimationStates);
        }
    }
}
