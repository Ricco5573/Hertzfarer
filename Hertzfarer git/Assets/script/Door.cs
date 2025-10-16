using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField]
    private Transform closedPos, openPos;
    private bool locked = false;
    [SerializeField]
    private bool closed = true;
    [Header("Variables")]
    [SerializeField]
    private float moveSpeed = 1f;
    private Transform targetPos;
    // Update is called once per frame
    void Update()
    {
        targetPos = closed ? closedPos : openPos;
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos.position, moveSpeed);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetPos.rotation, moveSpeed);   

    }
    public void Open()
    {
        if (locked) return;
        closed = !closed;
    }
}
