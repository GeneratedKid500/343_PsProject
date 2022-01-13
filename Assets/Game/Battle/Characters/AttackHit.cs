using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackHit : MonoBehaviour
{
    AudioSource m_AudioSource;
    [SerializeField] AudioClip attackClipA;
    [SerializeField] PlayBattleManager pbm;
    [SerializeField] string attackName;
    [SerializeField] string specialName;
    StatStorage attacker;
    StatStorage defender;

    void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        pbm = GameObject.Find("Center").GetComponent<PlayBattleManager>();
    }

    public void WhenAttackHit(int attackPower)
    {
        pbm.CalculateAttackPower(attacker, defender, attackPower);
        m_AudioSource.PlayOneShot(attackClipA);
    }

    public string SetAttackDefend(StatStorage attack, StatStorage defend, char moveType)
    {
        attacker = attack;
        defender = defend;

        if (moveType == 'a') return attackName;
        else return specialName;

    }
}
