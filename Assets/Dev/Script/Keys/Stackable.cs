using Sirenix.OdinInspector;
using UnityEngine;

public class Stackable : MonoBehaviour
{
    [SerializeField] 
    private BoxCollider col;
    [SerializeField]
    private GemType gemType;
    public GemType GemType => gemType;
    [SerializeField]
    private bool _isCollected = false;
    public bool IsCollected => _isCollected;
    [SerializeField]
    private bool _isCollectable = false;
    public bool IsCollectable => _isCollectable;
    [ShowInInspector]
    private int _priceValue;

    private void OnEnable()
    {
        ResetProps();
    }
    public void SetType(GemType type)
    {
        gemType = type;
    }

    public void SetIsCollected(bool isCollected)
    {
        _isCollected = isCollected;
    }

    public void SetIsCollectable(bool isCollectable)
    {
        _isCollectable = isCollectable;
    }

    public void CalculatePriceValue(int SalePrice)
    {
        _priceValue = (int) (this.transform.localScale.z * 100) + SalePrice;
    }

    public void ResetProps()
    {
        _isCollectable = this.transform.localScale.z <= 0.25f ? false : true;
        _isCollected = false;
    }

}