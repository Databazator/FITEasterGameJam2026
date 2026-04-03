using System;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float AttractorRadius;
    public float GravityStrength;

    private GravityAttractee _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<GravityAttractee>();
    }

    internal void UpdateAttractor(GravityAttractee player)
    {
        //if (player.CurrentAttractor != this && player.SuppressedAttactor != this)
        //{
        //    Debug.Log("Started Attracting Player");
        //    player.SetSuppressedAttractor(null);
        //}
        Vector3 gravityDirection = (transform.position - player.transform.position).normalized;
        Vector3 gravity = gravityDirection * GravityStrength;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        player.Attract(gravity, this, distance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttractorRadius);

        if(_player)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_player.transform.position, _player.transform.position + (transform.position - _player.transform.position).normalized);
        }
    }

}
