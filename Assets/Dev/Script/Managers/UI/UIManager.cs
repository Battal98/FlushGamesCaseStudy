using Commands;
using Controllers;
using Signals;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UIModules.Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField]
        private TextMeshProUGUI goldText;

        [SerializeField]
        private List<GameObject> popUpList = new List<GameObject>();

        [SerializeField]
        private GameObject storeObject;
        #endregion

        #region Private Variables

        [ShowInInspector]
        private List<CollectedGemCardsController> collectedGemCards = new List<CollectedGemCardsController>();
        private LevelPanelCommand _levelPanelCommand;
        private PopupCommands _popupCommands;

        #endregion

        #endregion

        private void Awake()
        {
            _levelPanelCommand = new LevelPanelCommand(ref goldText);
            _popupCommands = new PopupCommands(ref popUpList);
            _popupCommands.CloseAllPopups();

        }

        private void SetCollectedGemCardToList()
        {
            for (int i = 0; i < storeObject.transform.childCount; i++)
            {
                var objComponent = storeObject.transform.GetChild(i).GetComponent<CollectedGemCardsController>();
                collectedGemCards.Add(objComponent);
            }
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onUpdateGoldScoreText += OnUpdateGoldScoreText;
            CoreGameSignals.Instance.onSetCollectedGemData += OnSetCollectedGemDatas;
            UISignals.Instance.onChangeGemScoreAndCount += OnChangeGemScoreAndCount;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onSetCollectedGemData -= OnSetCollectedGemDatas;
            UISignals.Instance.onUpdateGoldScoreText -= OnUpdateGoldScoreText;
            UISignals.Instance.onChangeGemScoreAndCount -= OnChangeGemScoreAndCount;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Popup Jobs

        private void OpenPopup(PopupType popupType)
        {
            _popupCommands.OpenPopUp(popupType);
        }

        private void ClosePopup(PopupType popupType)
        {
            _popupCommands.ClosePopUp(popupType);
        }

        #endregion

        #region Button Jobs

        public void OnOpenStoreButton()
        {
            OpenPopup(PopupType.StorePopup);
        }

        public void OnCloseButton()
        {
            ClosePopup(PopupType.StorePopup);
        }

        #endregion

        private void OnChangeGemScoreAndCount(int gemCount, GemType type , int totalGold)
        {
            collectedGemCards[(int)type].ChangeGemCountAndGoldScore(gemCount, totalGold);
        }

        private void OnSetCollectedGemDatas(CollectedGemDatasList collectedGemDatas)
        {
            SetCollectedGemCardToList();
            for (int i = 0; i < collectedGemCards.Count; i++)
            {
                collectedGemCards[i].SetVariables(collectedGemDatas.CollectedGemDataList[i]);
            }
        }

        private void OnUpdateGoldScoreText(int moneyValue)
        {
            _levelPanelCommand.SetGoldScoreText(moneyValue);
        }
    }
}