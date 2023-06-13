using UnityEngine;

public class Stackable : MonoBehaviour
{
    [SerializeField] 
    private BoxCollider col;
    [SerializeField]
    private GemType gemType;

    public GemType GemType => gemType;

    public bool isCollected = false;

    public void SetType(GemType type)
    {
        gemType = type;
    }

}