using UnityEngine;

public class Seat : Interactable
{

    [Header("Assignables")]
    [SerializeField]
    private BoxCollider inside, outside;
    [SerializeField]
    private Transform seatPos, outsidePos;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Door door;
    [SerializeField]
    private GameObject car;

    private bool isInside = false, transitioning = false;

    public override void Interact()
    {
        isInside = !isInside;
        inside.enabled = !isInside;
        outside.enabled = isInside;
        transitioning = true;
        if (isInside)
        {
            player.transform.parent = car.transform;
            player.GetComponent<Playercontroller>().SetControlsEnabled(false);

            player.GetComponent<CapsuleCollider>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Rigidbody>().useGravity = false;
        }

    }
    public void Update()
    {
        if (transitioning)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, isInside ? seatPos.position : outsidePos.position, 0.05f);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, isInside ? seatPos.rotation : outsidePos.rotation, 1.25f);
            if(Vector3.Distance(player.transform.position, isInside ? seatPos.position : outsidePos.position) < 0.1f)
            {
                transitioning = false;
                door.Interact();
                if (!isInside)
                {
                    player.GetComponent<CapsuleCollider>().enabled = true;
                    player.GetComponent<Rigidbody>().isKinematic = true;
                    player.GetComponent<Rigidbody>().useGravity = true;
                    player.GetComponent<Playercontroller>().SetControlsEnabled(true);
                    player.transform.parent = null;
                }

            }
        }
    }
}
