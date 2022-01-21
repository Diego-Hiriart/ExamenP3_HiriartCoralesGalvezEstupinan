using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Controller : MonoBehaviour
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            this.transform.Translate(Vector3.up * speed * Time.deltaTime*jumpPower);
            this.JumpEffect();
        }
    }

    private void JumpEffect()
    {
        this.jumpSFX.Play();
    }

}
