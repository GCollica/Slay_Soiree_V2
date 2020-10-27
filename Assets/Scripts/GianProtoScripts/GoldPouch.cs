using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPouch : MonoBehaviour
{
    public int goldValue = 5;
    private Collider2D thisCollider;

    private float wakeUpTimer = 0f;
    private float wakeUpDuration = 1.5f;

    private bool wakingUp = false;


    // Start is called before the first frame update
    void Awake()
    {
        thisCollider = GetComponent<CircleCollider2D>();
        thisCollider.enabled = false;
        wakingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(wakingUp == true)
        {
            if(wakeUpTimer < wakeUpDuration)
            {
                wakeUpTimer += Time.deltaTime;
            }
            else
            {
                thisCollider.enabled = true;
                wakingUp = false;
            }

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().playerClass.currentGold += goldValue;
            Destroy(this.gameObject);
        }
    }
}
