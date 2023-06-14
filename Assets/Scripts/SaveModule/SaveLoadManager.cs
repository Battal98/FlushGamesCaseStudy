using UnityEngine;
using SaveLoadModule.Command;
using SaveLoadModule.Signals;
using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using Datas;
using Controllers;

namespace SaveLoadModule
{
    public class SaveLoadManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private LoadGameCommand _loadGameCommand;
        private SaveGameCommand _saveGameCommand;


        #endregion

        #endregion

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _loadGameCommand = new LoadGameCommand();
            _saveGameCommand = new SaveGameCommand();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveLoadSignals.Instance.onSaveCollectedGemData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadCollectedGemData += _loadGameCommand.Execute<CollectedGemDatasList>;            
            SaveLoadSignals.Instance.onSaveScoreData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadScoreData += _loadGameCommand.Execute<ScoreData>;
        }

        private void UnsubscribeEvents()
        {
            SaveLoadSignals.Instance.onSaveCollectedGemData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadCollectedGemData -= _loadGameCommand.Execute<CollectedGemDatasList>;
            SaveLoadSignals.Instance.onSaveScoreData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadScoreData -= _loadGameCommand.Execute<ScoreData>;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
    } 
}
