using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEnemyCollision : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gm.lcd.SetCarryPlayerPos(other.transform);
            gm.lcd.SetCarryCollidedEnemy(this.transform.parent);

            gm.ToBattle();
        }
    }
}
