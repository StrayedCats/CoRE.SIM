using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPanel : MonoBehaviour
{
    public GameObject panel_led_1;
    public GameObject panel_led_2;

    public Material red_material;
    public Material blue_material;

    private int counter;
    private int counter2;

    void Start()
    {  
       counter = 0;
       counter2 = 0;
    }

    void Update(){}

    void FixedUpdate(){
        counter2++;
        if(counter2 > 5){counter2 = 0;}
        else{return;}

        if(counter == 0){
            panel_led_1.GetComponent<MeshRenderer>().material = blue_material;
            panel_led_2.GetComponent<MeshRenderer>().material = blue_material;
            return;
        }
        
        if(counter % 2 == 0){
            panel_led_1.GetComponent<MeshRenderer>().material = blue_material;
            panel_led_2.GetComponent<MeshRenderer>().material = blue_material;

        }else{
            panel_led_1.GetComponent<MeshRenderer>().material = red_material;
            panel_led_2.GetComponent<MeshRenderer>().material = red_material;
        }
        counter -= 1;
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer == 8){
            counter = 5;
        }
    }
}
