using UnityEngine;
# if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Targeting))]
public class Sight : MonoBehaviour
{
    [Header("FOV")]
    public int angle = 90;
    public float rad = 5f;
    public Vector3 fromVector
    {
        get
        {
            float leftAngle = -angle / 2;
            leftAngle += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(leftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(leftAngle * Mathf.Deg2Rad));
        }
        //gets the left angle of the radius of which the enemy can see the player and then uses that to form the visual seen in editor FOV
    }

    private float distance;
    private float timer = 0;

    [HideInInspector] public Targeting targeting;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        targeting = GetComponent<Targeting>();
        targeting.enabled = false;
    }

    void FixedUpdate()
    {
        Vector3 directionVector = (transform.position - player.transform.position).normalized; // rot direction to player
        distance = Vector3.Distance(transform.position, player.transform.position); // distance from player

        float dotProduct = Vector3.Dot(directionVector, transform.forward);

        if (dotProduct < -0.5f && distance < rad)
        {
            Vector3 dirToPlayer = player.transform.position - transform.position;

            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, rad) && hit.transform.tag == "Player")
            {
                timer += Time.fixedDeltaTime;
                targeting.enabled = true;
            }
            else Disable();
        }
        else Disable();
    }

    void Disable()
    {
        targeting.enabled = false;
        timer = 0;
    }
}

#region Editor Drawing
#if UNITY_EDITOR
[CustomEditor(typeof(Sight))]
public class EditorFOV : Editor
{
    private void OnSceneGUI()
    {
        Sight sight = (Sight)target;
        Handles.color = new Color(1, 0, 0, 0.25f);
        Vector3 positionOnGround = new Vector3(sight.transform.position.x, 0, sight.transform.position.z);

        Handles.DrawSolidArc(positionOnGround, Vector3.up, sight.fromVector, sight.angle, sight.rad);
    }
}
#endif
#endregion