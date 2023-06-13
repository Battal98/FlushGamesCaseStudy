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
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

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
    [ShowInInspector]
    private Stackable _gemObject;

    private List<PlantedGemsData> _plantedGemsData;
    private PlantedGemsData data;
    private bool _isGrowing = false;
    private bool _isCollected = false;

    [ShowInInspector]
    private Tweener _gemTween;
    private Tweener _spriteTween;

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
        if (_isGrowing && !_isCollected)
            return;
        var obj = GetObject(ConvertGemTypeToPoolType());
        data = _plantedGemsData[(int)_gemType];
        var objComponent = obj.GetComponent<Stackable>();
        objComponent.SetType(_gemType);
        _gemObject = objComponent;
        _isGrowing = true;
        obj.transform.SetParent(targetTransform);
        obj.transform.localScale = Vector3.zero * 0.01f;
        obj.transform.localPosition = Vector3.zero;
        _gemTween = obj.transform.DOScale(Vector3.one, data.GrownTime).OnUpdate(() =>
        {
            if (!_gemObject.IsCollectable && obj.transform.localScale.z >= data.CollectibleGemScale)
            {
                spriteRendererFill.material.color = Color.green;
                _gemObject.SetIsCollectable(true);
                //TODO: Collectable olduðunun sinyalini yolla
            }
        });
        _spriteTween = spriteRendererFill.material.DOFloat(0, MaterialArc, data.GrownTime);
    }

    public Stackable GetGemObject()
    {
        return _gemObject;
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

    public void OnResetTile()
    {
        _gemTween.Kill();
        _spriteTween.Kill();
        _gemObject.CalculatePriceValue(data.SalePrice);
        _isGrowing = false;
        _isCollected = false;
        spriteRendererFill.material.SetFloat(MaterialArc, 360);
        spriteRendererFill.material.color = Color.red;
        _gemObject = null;
    }
}
