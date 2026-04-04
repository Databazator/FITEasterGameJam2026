using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityAttractee : MonoBehaviour
{
    public PlayerController PlayerController;
    public Vector3 CurrentGravity => _currentGravity;

    private Vector3 _currentGravity;

    private GravityAttractor _currentAttractor;
    public GravityAttractor CurrentAttractor => _currentAttractor;

    bool _isAttracted;
    public bool IsAttracted => _isAttracted;

    private GravityAttractor _suppressedAttractor;
    public GravityAttractor SuppressedAttactor => _suppressedAttractor;

    public void SetSuppressedAttractor(GravityAttractor attr) => _suppressedAttractor = attr;

    private List<(Vector3 gravForce, float distance, GravityAttractor attractor)> _currentAttractions = new List<(Vector3 force, float distance, GravityAttractor attractor)>();

    public void Attract(Vector3 gravitationalForce, GravityAttractor attractor, float distance)
    {
        _currentAttractions.Add((gravitationalForce, distance, attractor));
    }
    internal void HandleAttractionForces()
    {
        GravityAttractor lastAttractor = _currentAttractor;

        _isAttracted = false;
        _currentAttractor = null;
        _currentGravity = Vector3.zero;
        if (_currentAttractions.Count == 0)
        {            
            return;
        }

        var attractions = _currentAttractions.OrderByDescending(x => x.gravForce.sqrMagnitude * 1f / x.distance);
        foreach (var attraction in attractions)
        {
            if (attraction.attractor != _suppressedAttractor)
            {
                if (attraction.attractor.AttractorRadius >= attraction.distance)
                {   
                    if(lastAttractor != attraction.attractor)
                    {
                        //entered new attractors sphere
                        Debug.Log("Entered new attractors sphere");
                        PlayerController.Movement.SetCanLaunch(true);
                        PlayerController.Movement.SetLaunched(false);
                        PlayerController.Movement.CamEffects.FadeSpeedlines(1.5f);
                        PlayerController.Movement.CamEffects.UnsetFOV(1f);
                    }
                    _currentAttractor = attraction.attractor;
                    _isAttracted = true;
                    _currentGravity = attraction.gravForce;
                    //Debug.Log("Updating Attractor's gravity force");
                    break;
                }
            }
        }        

        _currentAttractions.Clear();
    }

    private void Awake()
    {
        if(!PlayerController) PlayerController = GetComponent<PlayerController>();
    }


}
