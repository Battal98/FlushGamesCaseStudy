using Datas;
using Datas.Scriptables;
using SaveLoadModule.Enums;
using SaveLoadModule.Signals;
using Signals;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData _scoreData;

    private int _totalGoldScore;
    private const int _uniqeID = 2521263;

    private const string DefaultDataPath = "Data/CD_Score";

    private void Start()
    {
        InitLevelData();
    }
    private void InitLevelData()
    {
        _scoreData = GetScoreData();
        if (!ES3.FileExists(_scoreData.GetKey().ToString() + $"{_uniqeID}.es3"))
        {
            if (!ES3.KeyExists(_scoreData.GetKey().ToString()))
            {
                _scoreData = GetScoreData();
                SaveGameScoreData(_scoreData, _uniqeID);
            }
        }
        LoadGameScoreData();
    }


    private ScoreData GetScoreData()
    {
        return Resources.Load<CD_Score>(DefaultDataPath).ScoreData;
    }
    private void LoadGameScoreData()
    {
        _scoreData = SaveLoadSignals.Instance.onLoadScoreData?.Invoke(SaveLoadType.ScoreData, _uniqeID);
        _totalGoldScore = _scoreData.TotalGoldScore;
        SetScoreText();
    }

    private void SetScoreText()
    {
        UISignals.Instance.onUpdateGoldScoreText?.Invoke(_totalGoldScore);
    }

    #region Event Subscriptions

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onUpdateGoldScore += OnUpdateGoldScore;
    }
    private void UnsubscribeEvents()
    {
        CoreGameSignals.Instance.onUpdateGoldScore -= OnUpdateGoldScore;
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #endregion

    private void OnUpdateGoldScore(int _amount)
    {
        _totalGoldScore += _amount;
        SetScoreText();
    }
    private void SaveGameScoreData(ScoreData scoreData, int uniqeID) => SaveLoadSignals.Instance.onSaveScoreData?.Invoke(scoreData, uniqeID);
    private void OnApplicationQuit()
    {
        _scoreData.TotalGoldScore = _totalGoldScore;
        SaveGameScoreData(_scoreData, _uniqeID);
    }
} 

