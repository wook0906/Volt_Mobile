using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController
{
    private Ray _ray;
    private Vector3 _origin;
    private Vector3 _direction;
    private RaycastHit _hit;
    private float _rayDistance;
    private LayerMask _layerMask;
    private List<string> _targetTags;

    public Color rayColor = Color.red;

    public RayController(Ray ray, float distance, LayerMask layerMask, params string[] targetTags)
    {
        this._ray = ray;
        this._origin = _ray.origin;
        this._direction = _ray.direction;
        this._rayDistance = distance;
        this._layerMask = layerMask;
        this._targetTags = new List<string>();
        this._targetTags.AddRange(targetTags);
    }

    public RayController(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask, params string[] targetTags)
    {
        Ray ray = new Ray(origin, direction);
        this._origin = origin;
        this._direction = direction;
        this._ray = ray;
        this._rayDistance = distance;
        this._layerMask = layerMask;
        this._targetTags = new List<string>();
        this._targetTags.AddRange(targetTags);
    }

    public bool Raycast()
    {
        Debug.DrawRay(_origin, _direction * _rayDistance, rayColor, 3f);
        if(Physics.Raycast(_ray, out _hit, _rayDistance, _layerMask))
        {
            if (_targetTags.Count == 0)
                return true;

            foreach (string tag in _targetTags)
            {
                if (_hit.transform.CompareTag(tag) == true)
                    return true;
            }
        }
        return false;
    }

    public GameObject GetHitedGameObject()
    {
        return _hit.transform.gameObject;
    }
}
