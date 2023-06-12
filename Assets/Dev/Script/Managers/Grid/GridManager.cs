using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridSurface
{
    XY,
    XZ,
    YZ,
}
public class GridManager : MonoBehaviour
{

    [SerializeField] private Vector2 gridSize;
    [SerializeField] private Vector2 gridOffsets;
    [SerializeField] private GridSurface gridSurface;
    [SerializeField] private GameObject gridObject;
    [SerializeField] private Transform gridPivotTarget;
    [SerializeField] private int direction;
    private float gridPivotCalculate;

    private const int XYRot = 0;
    private const int XZRot = 90;
    private const int YZRot = 90;

    private List<GameObject> gridList = new List<GameObject>();

    private void Awake()
    {
        if (!ValidateGridSize()) return;

        CreateGrid();
    }

    private bool ValidateGridSize()
    {
        return gridSize.x > 0 && gridSize.y > 0;
    }

    private void CreateGrid()
    {
        OnCreateGrid();
    }

#if UNITY_EDITOR
    [Button]
#endif
    private void OnCreateGrid()
    {
        if (gridSize.x <= 0 || gridSize.y <= 0)
            return;

        int gridCount = (int)(gridSize.x * gridSize.y);

        // Pivot calculation is not shown in the provided code, assuming it remains the same
        gridPivotCalculate = CheckPivotPosition(gridCount);

        for (int i = 0; i < gridCount; i++)
        {
            var modX = i % (int)gridSize.x;
            var divide = i / (int)gridSize.x;
            var modY = divide % (int)gridSize.y;
            Vector3 position;
            Quaternion rotation;

            switch (gridSurface)
            {
                case GridSurface.XY:
                    position = CalculatePosition(gridPivotTarget.position, direction * modX, modY, 0);
                    rotation = Quaternion.Euler(0, 0, XYRot);
                    break;

                case GridSurface.XZ:
                    position = CalculatePosition(gridPivotTarget.position, direction * modX, 0+0.02f, modY);
                    rotation = Quaternion.Euler(XZRot, 0, 0);
                    break;

                case GridSurface.YZ:
                    position = CalculatePosition(gridPivotTarget.position, 0, direction * modX, modY);
                    rotation = Quaternion.Euler(0, YZRot, 0);
                    break;

                default:
                    throw new ArgumentException("Invalid grid surface.");
            }

            CreateAndSetupGridObject(position, rotation);
        }
    }
    private float CheckPivotPosition(int gridSize)
    {
        if (gridSize % 2 == 0)
            return gridSize / 2 - 0.5f;
        return gridSize / 2;
    }
    private Vector3 CalculatePosition(Vector3 pivot, float modX, float modY, float modZ)
    {
        return new Vector3(
            modX * gridOffsets.x + pivot.x,
            modY * gridOffsets.y + pivot.y,
            modZ * gridOffsets.y + pivot.z // Assuming this is meant to be gridOffsets.y based on original code
        );
    }

    private void CreateAndSetupGridObject(Vector3 position, Quaternion rotation)
    {
        var obj = Instantiate(gridObject);
        obj.transform.SetParent(this.transform);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        gridList.Add(obj);
    }


    public struct GridSurfaceData
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public GridSurfaceData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}

