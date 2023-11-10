using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetBotController : MonoBehaviour
{
    private NavMeshAgent bot_nav;
    private Vector3 target_pos;
    // Start is called before the first frame update
    void Start()
    {
        bot_nav = this.GetComponent<NavMeshAgent>();
        target_pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
