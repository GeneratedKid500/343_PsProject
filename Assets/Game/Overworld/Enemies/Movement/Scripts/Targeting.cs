using UnityEngine;

public class Targeting : MonoBehaviour
{
    //editor vars
    public float rotSpeed = 1f;

    //system vars
    private GameObject target;
    private Vector3 directionToTarget;
    private float angleToTarget;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player"); //gets player game object
    }

    void Update()
    {
        directionToTarget = target.transform.position - transform.position; //calculates direction to player
        angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg; //gets the angle it needs to turn around to face the player

        transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z),
                                              Quaternion.Euler(transform.eulerAngles.x, angleToTarget, transform.eulerAngles.z), rotSpeed * Time.deltaTime);
    }
}
