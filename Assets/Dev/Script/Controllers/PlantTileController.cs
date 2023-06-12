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
    private bool _isGrowing = false;
    private bool _isCollectable = false;
    private bool _isCollected = true;

    private const string DataPath = "Data/CD_PlantedGemData"; 
    private const string MaterialArc = "_Arc1"; 
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
        if (_isGrowing || !_isCollected)
            return;
        OnResetTile();
        var obj = GetObject(ConvertGemTypeToPoolType());
        var data = _plantedGemsData[(int)_gemType];
        obj.transform.SetParent(targetTransform);
        obj.transform.localScale = Vector3.zero * 0.01f;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.DOScale(Vector3.one, data.GrownTime).OnUpdate(()=>
        {
            if (!_isCollectable && obj.transform.localScale.z >= data.CollectibleGemScale)
            {
                _isCollectable = true;
                spriteRendererFill.material.color = Color.green;
                //TODO: Collectable olduðunun sinyalini yolla
            }
        }).OnComplete(()=> _isGrowing = false);
        spriteRendererFill.material.DOFloat(0, MaterialArc, data.GrownTime);
    }

    [Button]
    public void IsCollected(bool isCollected)
    {
        _isCollected = isCollected;
        if (isCollected)
        {
            OnStartGrown();
        }
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

    private void OnResetTile()
    {
        _isGrowing = true;
        _isCollectable = false;
        _isCollected = false;
        spriteRendererFill.material.SetFloat(MaterialArc, 360);
        spriteRendererFill.material.color = Color.red;
    }
}
