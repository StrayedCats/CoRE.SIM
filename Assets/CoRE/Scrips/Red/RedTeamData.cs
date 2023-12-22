using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTeamData : MonoBehaviour
{
    public float killcount;
    public float buffcount;
    public float buff;
    public float before_buff;
    public float buff_time1;
    public float buff_time2;
    //ƒoƒt‚Ì‚©‚¯‚ê‚é‰ñ”
    public float buff_attack;
    public float buff_heal;

    void Start()
    {
        killcount = 0;
        buffcount = 0;
        buff = 1;
        before_buff = buffcount;
        buff_attack = 2;
        buff_heal = 1;
    }

    void FixedUpdate()
    {
        switch (buffcount)
        {
            case 0:
                buff = 1;
                break;
            case 1:
                buff = 2;
                break;
            case 2:
                buff = 4;
                break;
        }

        if(buffcount > before_buff)
        {
            buff_attack -= 1;
            if(buff_time1 == 0)
            {
                buff_time1 =+ 1;
            }
            else
            {
                buff_time2 =+ 1;
            }

            before_buff = buffcount;
        }

        if(buff_time1 >= 1)
        {
            buff_time1 += Time.deltaTime;
            if(buff_time1 >= 31)
            {
                buff_time1 = 0;
                buffcount--;
                before_buff = buffcount;
            }
        }
        if( buff_time2 >= 1)
        {
            buff_time2 += Time.deltaTime;
            if (buff_time2 >= 31)
            {
                buff_time2 = 0;
                buffcount--;
                before_buff = buffcount;
            }
        }
                
    }
}
