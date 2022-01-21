using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    protected NavMeshAgent enemyAgent;

    bool timer = false;
    float tTime;

    bool isChasing;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyAgent != null && player != null && isChasing )
        {
            timer = true;
            enemyAgent.SetDestination(player.transform.position);         
        }

        if(timer)
        {
            tTime += Time.deltaTime;
            Debug.Log(tTime);
        }
        else
        {
            tTime = 0;
        }

        // Si han pasado 5 segundos del tiempo de chase
        if(tTime > 5f)
        {
            isChasing = false;
            timer = false;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            isChasing = true;
            player = other.gameObject;
        }
    }
}
