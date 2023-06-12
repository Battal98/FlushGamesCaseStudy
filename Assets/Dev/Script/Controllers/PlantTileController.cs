using PoolModule.Enums;
using UnityEngine;
using PoolModule.Interfaces;
using PoolModule.Signals;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using Datas;
using Datas.Scriptables;
using System.Collections.Generic;

public enum GemType
{
    Gem1,Gem2, Gem3
}
public class PlantTileController : MonoBehaviour, IGetPoolObject
{
    #region Serializable Variables

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private SpriteRenderer spriteRendererFill;

    #endregion

    #region Private Variables
    [ShowInInspector]
    private GemType _gemType;

    private List<PlantedGemsData> _plantedGemsData;
    private const string DataPath = "Data/CD_PlantedGemData"; 
    #endregion

    private void Awake()
    {
        _plantedGemsData = GetPlantedGemData();
    }

    private List<PlantedGemsData> GetPlantedGemData()
    {
        return Resources.Load<CD_PlantedGemData>(DataPath).PlantedGemsDatas;
    }

    [Button]
    private void OnStartGrown()
    {
        var obj = GetObject(ConvertGemTypeToPoolType());
        obj.transform.SetParent(targetTransform);
        obj.transform.localScale = Vector3.zero * 0.01f;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.DOScale(Vector3.one, _plantedGemsData[(int )_gemType].GrownTime);
        spriteRendererFill.material.DOFloat(0, "_Arc1", _plantedGemsData[(int)_gemType].GrownTime);
    }

    private GemType GetRandomPoolType()
    {
        _gemType = _gemType.RandomEnumValue();
        return _gemType;
    }

    private PoolType ConvertGemTypeToPoolType()
    {
        string enumName = GetRandomPoolType().ToString();
        var convertedValue = (PoolType)Enum.Parse(typeof(PoolType), enumName);
        return convertedValue;
    }

    public GameObject GetObject(PoolType poolType)
    {
        return PoolSignals.Instance.onGetObjectFromPool?.Invoke(poolType);
    }
}
