using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WallBlockPoint : MonoBehaviour
{
    public enum BlockDirection
    {
        Left,
        Right,
        Forward,
        Backward,
        ForwardLeft,
        ForwardRight,
        BackwardLeft,
        BackwardRight
    }

    public BlockDirection[] blockDirections;

    private Vector3[] directions;
    [SerializeField]
    private Color xAxisColor = Color.red;
    [SerializeField]
    private Color zAxisColor = Color.blue;
    [SerializeField]
    private Color xzAxisColor = Color.red + Color.blue;

    private void Start()
    {
        int i = 0;
        directions = new Vector3[blockDirections.Length];
        foreach (var blockDir in blockDirections)
        {
            switch (blockDir)
            {
                case BlockDirection.Backward:
                    directions[i] = Vector3.back;
                    break;
                case BlockDirection.BackwardLeft:
                    directions[i] = (Vector3.back + Vector3.left).normalized;
                    break;
                case BlockDirection.BackwardRight:
                    directions[i] = (Vector3.back + Vector3.right).normalized;
                    break;
                case BlockDirection.Forward:
                    directions[i] = Vector3.forward;
                    break;
                case BlockDirection.ForwardLeft:
                    directions[i] = (Vector3.forward + Vector3.left).normalized;
                    break;
                case BlockDirection.ForwardRight:
                    directions[i] = (Vector3.forward + Vector3.right).normalized;
                    break;
                case BlockDirection.Left:
                    directions[i] = Vector3.left;
                    break;
                case BlockDirection.Right:
                    directions[i] = Vector3.right;
                    break;
                default:
                    break;
            }
            i++;
        }
    }
    public bool IsBlock(Vector3 movingDir)
    {
        foreach (var dir in directions)
        {
            float angle = Vector3.Angle(dir, movingDir);
            if (angle <= 15f)
                return true;
        }
        return false;
    }
}
