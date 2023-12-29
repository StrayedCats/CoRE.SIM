using System;
using System.Collections;
using System.Collections.Generic;
using builtin_interfaces.msg;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using Time = UnityEngine.Time;

public class BlueBufferBotControl : MonoBehaviour
{
    public SettingMenuAgent agent;
    private NavMeshAgent bot_nav;
    private Vector3 target_pos;
    private Vector3 initPos;

    [SerializeField]
    [Tooltip("èÑâÒÇ∑ÇÈínì_ÇÃîzóÒ")]
    private Transform[] waypoints;
    private int currentWaypointIndex;

    [SerializeField] private Transform RP;
    [SerializeField] private Transform NowPS;
    [SerializeField] private float ammo;
    public float HP;
    public bool invincible;
    public bool invincible_hit;

    [SerializeField] private bool Torch;
    [SerializeField] private Transform BuffPoint;
    [SerializeField] private Transform TorchPoint;
    [SerializeField] private float Get_Torch;
    [SerializeField] private float buff_start_count;
    [SerializeField] private float distance;
    public CountDownTimer totalTime;

    //0ÇÕñ≥ÇµÅA1ÇÕattackÅA2ÇÕheal
    public float buff_kind;

    public BlueAttackerBotControl attacker1;
    public BlueAttackerBotControl attacker2;
    public BlueBufferBotControl buffer;

    public BlueTeamData buff;
    public RedTeamData red;
    private bool kill;

    public GameObject[] child;

    [SerializeField]
    private float catch_time;
    [SerializeField]
    private float down_time;
    [SerializeField]
    private float invincible_time;
    [SerializeField]
    private float hit_cd;
    public BlueBotBuffAttack HitC;

    //ë≈Ç¬ä‘äu
    public float hit_CT;

    public void resetPos()
    {
        target_pos = initPos;
        bot_nav.SetDestination(target_pos);
        this.transform.position = new Vector3(6.13700008f, 0, -1.28299999f);
        this.transform.rotation = new Quaternion(0, 0, 0, 1);
    }

    void Start()
    {
        bot_nav = this.GetComponent<NavMeshAgent>();
        target_pos = this.transform.position;
        initPos = this.transform.position;
        ammo = 20;
        HP = 40;
        invincible = false;
        bot_nav.SetDestination(TorchPoint.position);
        buff_kind = 1;

        hit_CT = 2f;
        catch_time = 5f;
    }

    void FixedUpdate()
    {
        if (totalTime.totalTime == 150)
        {
            buff_kind = 1;
        }

        distance = Vector3.Distance(transform.position,BuffPoint.position);

        if (attacker1.HP <= 20 && attacker2.HP <= 20 && buffer.HP <= 20)
        {
            buff_kind = 2;
        }

        if (agent.isShow)
        {
            return;
        }

        //ïúäàå„ÇÃñ≥ìG
        if (invincible == true)
        {
            invincible_time += Time.deltaTime;
            if (invincible_time > 5f)
            {
                invincible_time = 0;
                invincible = false;
            }
        }

        //åÇÇΩÇÍÇΩÇ†Ç∆ÇÃñ≥ìG
        if (invincible_hit == true)
        {
            invincible_time += Time.deltaTime;
            if (invincible_time > 0.3f)
            {
                invincible_time = 0;
                invincible_hit = false;
            }
        }

        //HPÇ™0Ç©ämîF
        if (HP <= 0)
        {
            if (HP < 0)
            {
                HP = 0;
            }
            if (kill == false)
            {
                red.killcount += 1;
                kill = true;
            }
            for (int i = 0; i < 6; i++)
            {
                child[i].gameObject.layer = LayerMask.NameToLayer("Down-Robot");
            }
            bot_nav.SetDestination(NowPS.position);
            down_time += Time.deltaTime;
            if (down_time >= 60f)
            {
                HP = 30;
                down_time = 0;
                invincible = true;
                kill = false;

                for (int i = 0; i < 6; i++)
                {
                    child[i].gameObject.layer = LayerMask.NameToLayer("Red-Robot");
                }
            }
        }
        //èºñæälìæà íuÇ…Ç¢ÇÈÇ©ämîF
        else if (buff.buff_attack > 0 && buff_kind == 1)
        {
            if (Torch == false)
            {
                bot_nav.SetDestination(TorchPoint.position);
                if (this.transform.position == TorchPoint.position)
                {
                    Get_Torch += Time.deltaTime;
                    if (Get_Torch >= 3f)
                    {
                        Get_Torch = 0;
                        Torch = true;
                        bot_nav.SetDestination(BuffPoint.position);
                    }
                }
            }
            else if (Torch == true)
            {
                bot_nav.SetDestination(BuffPoint.position);
                if (distance < 1)
                {
                    buff_start_count += Time.deltaTime;
                    if (buff_start_count >= 3f)
                    {
                        buff_start_count = 0;
                        buff.buffcount++;
                        Torch = false;
                        bot_nav.SetDestination(waypoints[currentWaypointIndex].position);

                        buff_kind = 0;
                    }
                }
            }
        }
        else if (buff.buff_heal > 0 && buff_kind == 2)
        {
            if (Torch == false)
            {
                bot_nav.SetDestination(TorchPoint.position);
                if (this.transform.position == TorchPoint.position)
                {
                    Get_Torch += Time.deltaTime;
                    if (Get_Torch >= 3f)
                    {
                        Get_Torch = 0;
                        Torch = true;
                        bot_nav.SetDestination(BuffPoint.position);
                    }
                }
            }
            else if (Torch == true)
            {
                bot_nav.SetDestination(BuffPoint.position);
                if (distance < 1)
                {
                    buff_start_count += Time.deltaTime;
                    if (buff_start_count >= 3f)
                    {
                        buff_start_count = 0;
                        Torch = false;
                        bot_nav.SetDestination(waypoints[currentWaypointIndex].position);

                        attacker1.HP = 40;
                        attacker2.HP = 40;
                        buffer.HP = 40;

                        buff.buff_heal -= 1;
                        buff_kind = 0;
                    }
                }
            }
        }
        //íeï‚è[à íuÇ…Ç¢ÇÈÇ©ämîF
        else if (catch_time > 0f && this.transform.position == RP.position)
        {
            catch_time -= Time.deltaTime;
            if (catch_time <= 0f)
            {
                catch_time = 0f;
                ammo = 20;
                bot_nav.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
        //íeÇÃêîÇ™0Ç©ämîF
        else if (ammo < 1)
        {
            bot_nav.SetDestination(RP.position);
            catch_time = 5f;
        }
        //íeÇåÇÇ¬ÉNÅ[ÉãÉ^ÉCÉÄ
        else if (HitC.hitC == 2)
        {
            hit_cd += Time.deltaTime;
            if (hit_cd > hit_CT)
            {
                HitC.hitC = 0;
                hit_cd = 0f;
            }
            else
            {
                if (bot_nav.remainingDistance <= bot_nav.stoppingDistance)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    bot_nav.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
        }
        //íeÇåÇÇ¬Ç∆Ç´Ç…àÍéûí‚é~
        else if (HitC.hitC == 1)
        {
            ammo--;
            HitC.hitC = 2;
            bot_nav.SetDestination(NowPS.position);
        }
        //éüÇÃà íuÇ…à⁄ìÆ
        else
        {
            if (bot_nav.remainingDistance <= bot_nav.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                bot_nav.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }
}
