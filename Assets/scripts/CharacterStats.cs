﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CharacterStats : MonoBehaviour {

    public float health = 100f;
    bool dealDamage;
    bool substractOnce;
    bool dead;

    public float damageTimer = .4f;
    WaitForSeconds damageT;

    Animator anim;

    public GameObject sliderPrefab;

    Slider healthSlider;

    RectTransform healthTrans;

	void Start ()
    {
        damageT = new WaitForSeconds(damageTimer);
        anim = GetComponent<Animator>();

        GameObject slid = Instantiate(sliderPrefab, transform.position, Quaternion.identity) as GameObject;
        slid.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        healthSlider = slid.GetComponentInChildren<Slider>();
        healthTrans = slid.GetComponent<RectTransform>();
	}

    

    void Update ()
    {
        healthSlider.value = health / 100;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        healthTrans.transform.position = screenPoint;

        if (dealDamage)
        {
            if (!substractOnce)
            {
                health -= 30;
                anim.SetTrigger("Hit");
                substractOnce = true;
            }
            StartCoroutine("CloseDamage");
        }
        if(health < 0)
        {
            if (!dead)
            {
                anim.enabled = false;
                healthTrans.gameObject.SetActive(false);
                dealDamage = true;

                GetComponent<CapsuleCollider>().enabled = false;
                // GetComponent<Rigidbody>().isKinematic = true;
                Destroy(gameObject, 3f);

                if (GetComponent<StelsAI>())
                {
                    GetComponent<StelsAI>().enabled = false;
                    GetComponent<NavMeshAgent>().enabled = false;
                    anim.SetBool("Dead", true);
                    anim.CrossFade("Death", .5f);
                    Destroy(gameObject, 5f);
                }
                if (GetComponent<EnemyRobotAI>())
                {
                    GetComponent<EnemyRobotAI>().enabled = false;
                    GetComponent<NavMeshAgent>().enabled = false;
                    anim.SetBool("Dead", true);
                    anim.CrossFade("Death", .5f);
                    Destroy(gameObject, 5f);
                }
                if (GetComponent<EnemyAI>())
                {
                    GetComponent<EnemyAI>().enabled = false;
                    GetComponent<NavMeshAgent>().enabled = false;
                   anim.SetBool("Dead", true);
                   anim.CrossFade("Death", .5f);
                   Destroy(gameObject, 5f);

                }
                else
                {
                    GetComponent<PlayerInput>().enabled = false;
                    GetComponent<PlayerMovement>().enabled = false;
                    GetComponent<PlayerAttack>().enabled = false;
                }
                dead = true;
            }
            
        }
	}

    public void checkToApplyDamage()
    {
        if (!dealDamage)
        {
            dealDamage = true;
        }
    }
    IEnumerator CloseDamage()
    {
        yield return damageT;
        dealDamage = false;
        substractOnce = false;

    }
}
