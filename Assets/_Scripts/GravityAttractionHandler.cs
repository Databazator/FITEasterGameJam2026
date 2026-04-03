using System.Collections.Generic;
using UnityEngine;

public class GravityAttractionHandler : MonoBehaviour
{
    public List<GravityAttractor> Attractors;
    public GravityAttractee Player;

    private void Awake()
    {
        var attractors = FindObjectsByType<GravityAttractor>();
        foreach (var attractor in attractors)
        {
            if(!Attractors.Contains(attractor))
                Attractors.Add(attractor);
        }

        if (!Player)
            Player = FindAnyObjectByType<GravityAttractee>();
    }
    public void UpdateHandler()
    {
        foreach (var attractor in Attractors)
            attractor.UpdateAttractor(Player);

        Player.HandleAttractionForces();
    }
}
