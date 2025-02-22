using System.Collections;
using System.Collections.Generic;
using builtin_interfaces.msg;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Time = UnityEngine.Time;

public class RedAttackerBotControl : MonoBehaviour
{
    public SettingMenuAgent agent;
    private NavMeshAgent bot_nav;
    private Vector3 target_pos;
    private Vector3 initPos;

    [SerializeField]
    [Tooltip("巡回する地点の配列")]
    private Transform[] waypoints;
    private int currentWaypointIndex;

    [SerializeField] private Transform RP;
    [SerializeField] private Transform NowPS;
    [SerializeField] private float ammo;
    public float HP;
    public bool invincible;
    public bool invincible_hit;

    public BlueTeamData blue;
    [SerializeField] private bool kill;

    public GameObject[] child;

    [SerializeField]
    private float catch_time;
    [SerializeField]
    private float down_time;
    [SerializeField]
    private float invincible_time;
    [SerializeField]
    private float hit_cd;
    public RedBotAttak HitC;

    //打つ間隔
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
        bot_nav.SetDestination(waypoints[0].position);

        hit_CT = 2f;
        catch_time = 5f;
    }

    void FixedUpdate()
    {
        if (agent.isShow)
        {
            return;
        }

        //復活後の無敵
        if (invincible == true)
        {
            invincible_time += Time.deltaTime;
            if (invincible_time > 5f)
            {
                invincible_time = 0;
                invincible = false;
            }
        }

        //撃たれたあとの無敵
        if (invincible_hit == true)
        {
            invincible_time += Time.deltaTime;
            if (invincible_time > 0.3f)
            {
                invincible_time = 0;
                invincible_hit = false;
            }
        }

        //HPが0か確認
        if (HP <= 0)
        {
            if (HP < 0)
            {
                HP = 0;
            }
            if (kill == false)
            {
                blue.killcount += 1;
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
        //弾補充位置にいるか確認
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
        //弾の数が0か確認
        else if (ammo < 1)
        {
            bot_nav.SetDestination(RP.position);
            catch_time = 5f;
        }
        //弾を撃つクールタイム
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
        //弾を撃つときに一時停止
        else if (HitC.hitC == 1)
        {
            ammo--;
            HitC.hitC = 2;
            bot_nav.SetDestination(NowPS.position);
        }
        //次の位置に移動
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
