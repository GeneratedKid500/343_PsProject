using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputAnimators : MonoBehaviour
{
    GameManager gm;
    PlayBattleManager pbm;
    BattleMenuController bmc;
    Animator animator;
    Rigidbody rb;
    
    // can player input
    bool canInput = true;

    //original position & rotation
    Transform parentBase;
    float timeCount;
    float seconds;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bmc = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<BattleMenuController>();
        pbm = GameObject.Find("Center").GetComponent<PlayBattleManager>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        parentBase = transform.parent.transform;
    }

    void Update()
    {
        timeCount = Time.deltaTime;
    }

    public void Attack(Transform target)
    {
        if (canInput)
        {
            canInput = false;
            Target(target);
            animator.SetBool("MovingTo", true);

            StartCoroutine(MoveTo("AttackA", target));
        }
    }

    public void Special(Transform target, bool isMoving = true)
    {
        Debug.Log("Special");
        if (canInput)
        {
            canInput = false;
            Target(target);
            if (isMoving)
            {
                animator.SetBool("MovingTo", true);

                StartCoroutine(MoveTo("AttackB", target));
            }
            else
            {
                animator.SetBool("AttackB", true);

                StartCoroutine(WaitForCam(4f));
            }
        }
    }

    IEnumerator MoveTo(string setBool, Transform target)
    {
        float newX = 0;
        if (target.position.x > 0)
        {
            newX = target.position.x - 1;
        }
        else
        {
            newX = target.position.x + 1;
        }

        Vector3 newPos = new Vector3(newX, transform.position.y, target.position.z);
        while (transform.position != newPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, timeCount*5);
            yield return null;
        }
        Target(target);
        animator.SetBool(setBool, true);
        while (!animator.GetBool("MovingFrom"))
        {
            yield return null;
        }
        Target(parentBase);
        while (transform.position != parentBase.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, parentBase.position, timeCount*5);
            Debug.Log("stuck");
            yield return null;
        }

        if (parentBase.position.x > 0) { transform.rotation = Quaternion.Euler(parentBase.rotation.x, parentBase.rotation.y-90, parentBase.rotation.z); }
        else { transform.rotation = Quaternion.Euler(parentBase.rotation.x, parentBase.rotation.y+90, parentBase.rotation.z); }

        animator.SetBool("MovingFrom", false);

        yield return new WaitForSeconds(0.5f);

        SetNextTurn();
    }

    void Target(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position; 
        float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angleToTarget, transform.eulerAngles.z);
    }

    public void Item()
    {
        if (canInput)
        {
            canInput = false;
            animator.SetBool("UseItem", true);
            StartCoroutine(WaitForCam(2f));
        }
    }

    IEnumerator WaitForCam(float time)
    {
        while (seconds < time)
        {
            seconds += Time.deltaTime;
            yield return null;
        }
        SetNextTurn();
    }

    public void Flee()
    {
        if (canInput)
        {
            canInput = false;

            animator.SetBool("MovingTo", true);
            int rand = Random.Range(0, 100);
            StartCoroutine(RunAway(rand));
        }
    }

    IEnumerator RunAway(int rand)
    {
        Quaternion runAway = Quaternion.identity;
        if (parentBase.position.x > 0) { runAway = Quaternion.Euler(runAway.x, parentBase.rotation.y + 90, runAway.z); }
        else { runAway = Quaternion.Euler(runAway.x, parentBase.rotation.y - 90, runAway.z); }

        while (seconds < 2.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, runAway, timeCount);
            seconds += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        if (rand > 45)
        {
            // Flee Successful
            Debug.Log("run");
            rb.AddForce(transform.forward * 900);
            yield return new WaitForSeconds(2);

            pbm.SuccessfulFlee();
        }
        else
        {
            // Flee Failed
            bmc.SetText("Flee attempt failed!");

            if (parentBase.position.x > 0) { transform.rotation = Quaternion.Euler(parentBase.rotation.x, parentBase.rotation.y - 90, parentBase.rotation.z); }
            else { transform.rotation = Quaternion.Euler(parentBase.rotation.x, parentBase.rotation.y + 90, parentBase.rotation.z); }

            animator.SetBool("MovingTo", false);
        }
        SetNextTurn();
    }

    void SetNextTurn()
    {
        StopAllCoroutines();
        seconds = 0;
        canInput = true;
        pbm.NextTurn();
    }

    public void GetHurt(bool isPlayer)
    {
        if (isPlayer)
        {
            if (Random.Range(0, 2) == 0) animator.SetBool("Random", true);
            else animator.SetBool("Random", false);
        }
        animator.SetBool("Hurt", true);
    }

    public void Die()
    {
        animator.Play("Die");
    }

}
