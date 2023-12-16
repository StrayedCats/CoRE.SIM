using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetBotController : MonoBehaviour
{
    public SettingMenuAgent agent;
    private NavMeshAgent bot_nav;
    private Vector3 target_pos;

    private Vector3 initPos;
    
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
    }

    void FixedUpdate()
    {
        if (agent.isShow) { return; }

        bot_nav.SetDestination(target_pos);
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))  
            {
                target_pos = hit.point;
            }
        }
    }
}
