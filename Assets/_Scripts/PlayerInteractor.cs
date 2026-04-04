using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float InteractionDistance;
    public LayerMask InteractionsLayerMask;
    public Transform RaycastOrigin;

    public void UpdateInteractor(bool interact)
    {
        //Debug.Log($"Update Interactor called! {interact}");
        if (!interact) return;
        RaycastHit hit;
        if(Physics.Raycast(RaycastOrigin.position, RaycastOrigin.forward, out hit, InteractionDistance, InteractionsLayerMask))
        {
            //Debug.Log($"Interactor raycast hit! {hit.collider.gameObject}");
            EventInteractable ei = hit.collider.GetComponentInParent<EventInteractable>();
            if(ei && ei.CanInteract())
            {
                ei.Interact();
            }
        }
    }
}
