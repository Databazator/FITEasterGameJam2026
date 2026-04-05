using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float InteractionDistance;
    public LayerMask InteractionsLayerMask;
    public Transform RaycastOrigin;
    PlayerController playerCotroller;

    public EventInteractable LastHit;

    private void Start()
    {
        playerCotroller = GetComponent<PlayerController>();
    }

    public void UpdateInteractor(bool interact)
    {
        //Debug.Log($"Update Interactor called! {interact}");
        if (playerCotroller.Movement.IsInGravityAttractionZone) return;
        RaycastHit hit;
        if(Physics.Raycast(RaycastOrigin.position, RaycastOrigin.forward, out hit, InteractionDistance, InteractionsLayerMask))
        {
            
            //Debug.Log($"Interactor raycast hit! {hit.collider.gameObject}");
            EventInteractable ei = hit.collider.GetComponentInParent<EventInteractable>();
            if(LastHit == null)
            {
                GUIManager.Instance.ShowIndicator(true);
                LastHit = ei;
            }

            if(ei && interact && ei.CanInteract())
            {
                ei.Interact();
            }
        }
        else
        {
            GUIManager.Instance.ShowIndicator(false);
            LastHit = null;
        }
    }
}
