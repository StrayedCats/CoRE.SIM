using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBotBuffAttack : MonoBehaviour
{
    public GameObject disk;

    [NonSerialized]
    public float hitC;

    public BlueBufferBotControl blue;

    bool DiskAtck(double angle)
    {
        Vector3 raypoint = this.transform.position;
        raypoint.y = 0.5f;

        var vec = new Vector3(0f, 0f, 0f);
        vec.x = (float)Math.Cos((double)((Mathf.PI / 180f) * (angle + this.transform.rotation.eulerAngles.y)));
        vec.z = (float)Math.Sin((double)((Mathf.PI / 180f) * (angle + this.transform.rotation.eulerAngles.y)));
        RaycastHit hit;
        if (Physics.Raycast(raypoint, vec, out hit, 6f))
        {
            if (hit.collider.gameObject.layer == 16)
            {
                Debug.DrawRay(raypoint, vec * hit.distance, Color.green, 0.1f);
                var obj = Instantiate(disk, this.transform.position, Quaternion.Euler(this.transform.parent.eulerAngles.x, this.transform.parent.eulerAngles.y, 0));
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                float forceRandomness = UnityEngine.Random.Range(0.8f, 1.2f);
                Vector3 torqueRandomness = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                rb.AddForce(vec * 100 * forceRandomness);
                rb.AddTorque(torqueRandomness * 20f);

                hitC = 1;

                return true;
            }
            else
            {
                Debug.DrawRay(raypoint, vec * hit.distance, Color.blue, 0.1f);
            }
        }
        /*
        else
        {
            Debug.DrawRay(raypoint, vec * 6f, Color.red, 0.1f);
        }
        */
        return false;
    }

    private int counter = 0;
    void FixedUpdate()
    {
        if (blue.HP <= 0)
        {
            return;
        }

        counter++;
        if (counter > 30)
        {
            counter = 0;
        }
        else
        {
            return;
        }
        int lines = 10;
        for (int i = 0; i < lines; i++)
        {
            if (hitC == 0)
            {
                if (DiskAtck(i * (360 / lines)))
                {
                    return;
                }
            }
        }
    }
}
