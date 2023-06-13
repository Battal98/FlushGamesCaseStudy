using Data;
using Signals;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    [Serializable]
    public class CollectedGemDatas
    {
        public GemType GemType;
        public int GemCount;
        public int TotalGold;
    }

    public class StoreManager : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;
        [ShowInInspector]
        private List<CollectedGemDatas> data;
        private List<CollectedGemDatas> _collectedGemDatas;

        private const string DefaultDataPath = "Data/CD_CollectedGemData";
        private const string FolderName = "GemData";
        private const string FileName = "CollectedGemData";

        private void Awake()
        {
            data = data.LoadDataFromFile<List<CollectedGemDatas>>(FolderName, FileName);
            if (data == null)
            {
                data = Resources.Load<CD_CollectedGemData>(DefaultDataPath).CollectedGemData;
                data.SaveDataToFile(FolderName, FileName);
            }
            _collectedGemDatas = data;
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

        public void OnChangeGemCount(GemType gemType)
        {
            _collectedGemDatas[(int)gemType].GemCount++;
            data = _collectedGemDatas;
            data.SaveDataToFile(FolderName, FileName);
        }
    }
}
