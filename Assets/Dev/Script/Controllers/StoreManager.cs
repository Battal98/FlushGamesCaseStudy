using Data;
using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using SaveLoadModule.Signals;
using Signals;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    [Serializable]
    public class CollectedGemDatas
    {
        public GemType GemType;
        public string GemName;
        public GameObject SpriteObj;
        public int GemCount;
        public int TotalGold;
    }

    [Serializable]
    public class CollectedGemDatasList: ISavable
    {
        public List<CollectedGemDatas> CollectedGemDataList = new List<CollectedGemDatas>();

        private const SaveLoadType Key = SaveLoadType.CollectedGemData;

        public SaveLoadType GetKey()
        {
            return Key;
        }
        public CollectedGemDatasList()
        {
            
        }
    }

    public class StoreManager : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;
        [ShowInInspector]
        private CollectedGemDatasList data;
        [ShowInInspector]
        private List<int> _gemCounts = new List<int>();
        [ShowInInspector]
        private List<int> _totalGolds = new List<int>();

        private const string DefaultDataPath = "Data/CD_CollectedGemData";
        private const int _uniqeID = 251263;

        private void Start()
        {
            InitLevelData();
            CreateGemCountLists();
            CreateGemTotalGoldLists();
        }
        private void InitLevelData()
        {
            data = GetDefaultCollectedGemData();
            if (!ES3.FileExists(data.GetKey().ToString() + $"{_uniqeID}.es3"))
            {
                if (!ES3.KeyExists(data.GetKey().ToString()))
                {
                    data = GetDefaultCollectedGemData();
                    SaveCollectedGemData(data, _uniqeID);
                }
            }
            LoadCollectedGemData();
        }
        private void SaveCollectedGemData(CollectedGemDatasList collectedGemData, int uniqeID) => SaveLoadSignals.Instance.onSaveCollectedGemData?.Invoke(collectedGemData, uniqeID);
        private CollectedGemDatasList GetDefaultCollectedGemData() => Resources.Load<CD_CollectedGemData>(DefaultDataPath).CollectedGemData;
        private void LoadCollectedGemData()
        {
            data = SaveLoadSignals.Instance.onLoadCollectedGemData?.Invoke(SaveLoadType.CollectedGemData, _uniqeID);
            CoreGameSignals.Instance.onSetCollectedGemData?.Invoke(data);
        }
        #region Event Subscriptions 

        private void OnEnable()
        {
            SubscribeEvents(true);
        }
        private void SubscribeEvents(bool isRegister)
        {
            if (isRegister)
            {
                CoreGameSignals.Instance.onCalculateGemStackType += OnChangeGemCount;
            }
            else
            {
                CoreGameSignals.Instance.onCalculateGemStackType -= OnChangeGemCount;
            }
        }

        private void OnDisable()
        {
            SubscribeEvents(false);
        }

        #endregion

        public void ChangeColor(Color color)
        {
            Color _color = new Color(color.r, color.g, color.b, 0.5f); 
            meshRenderer.material.color = _color;
        }

        private void SetGemCountAndGold(Stackable stackable)
        {
            _gemCounts[(int)stackable.GemType] = data.CollectedGemDataList[(int)stackable.GemType].GemCount;
            _totalGolds[(int)stackable.GemType] = data.CollectedGemDataList[(int)stackable.GemType].TotalGold;
            UISignals.Instance.onChangeGemScoreAndCount?.Invoke(_gemCounts[(int)stackable.GemType], stackable.GemType, _totalGolds[(int)stackable.GemType]);
        }

        private void CreateGemCountLists()
        {
            if (_gemCounts.Count <=0 )
            {
                var enumLenght = Enum.GetValues(typeof(GemType)).Length;
                for (int i = 0; i < enumLenght; i++)
                {
                    _gemCounts.Add(0);
                }
            }
        }
        private void CreateGemTotalGoldLists()
        {
            if (_totalGolds.Count <= 0)
            {
                var enumLenght = Enum.GetValues(typeof(GemType)).Length;
                for (int i = 0; i < enumLenght; i++)
                {
                    _totalGolds.Add(0);
                }
            }
        }

        public void OnChangeGemCount( Stackable stackable)
        {
            SetGemCountAndGold(stackable);
            _gemCounts[(int)stackable.GemType]++;
            _totalGolds[(int)stackable.GemType] += stackable.GetPriceValue();
            data.CollectedGemDataList[(int)stackable.GemType].GemCount = _gemCounts[(int)stackable.GemType];
            data.CollectedGemDataList[(int)stackable.GemType].TotalGold = _totalGolds[(int)stackable.GemType];
            UISignals.Instance.onChangeGemScoreAndCount?.Invoke(_gemCounts[(int)stackable.GemType], stackable.GemType, _totalGolds[(int)stackable.GemType]);

        }


        private void OnApplicationQuit()
        {
            SaveCollectedGemData(data,_uniqeID);
        }
    }
}
