using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controller : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float jumpPower = 2;
    private AudioSource jumpSFX;

    // Start is called before the first frame update
    void Start()
    {
        this.jumpSFX = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left* speed*Time.deltaTime);
        }
            
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * speed* Time.deltaTime);
        }
            
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * speed* Time.deltaTime);
        }
            
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * speed* Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.Translate(Vector3.up * speed* Time.deltaTime*jumpPower);
            this.JumpEffect();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.Rotate(Vector3.down * speed*5 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate(Vector3.up * speed*5 * Time.deltaTime);
        }
    }

    private void JumpEffect()
    {
        this.jumpSFX.Play();
    }
}
