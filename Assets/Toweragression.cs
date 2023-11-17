using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Toweragression : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float visionRange = 10;
    [SerializeField] private float visionConeAngle = 180;
    [SerializeField] private TextMeshProUGUI stateIndicator;

       private Vector3 playerposition;
       

    private Vector3 oldRotation;
    private Vector3 targetRotation;
    private float rotateTimer = 0f; 

    private State state = State.Idle;

    float GetDistanceToPlayer()
    {
        return
            (player.transform.position - transform.position)
            .magnitude;
    }

    float GetAngleToPlayer()
    {
        Vector3 directionToPlayer =
            (player.transform.position - transform.position)
            .normalized;
        return Vector3.Angle(transform.forward, directionToPlayer);
    }

    bool SightLineObstructed()
    {
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        Ray ray = new Ray(
            transform.position,
            vectorToPlayer);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, vectorToPlayer.magnitude))
        {
            GameObject obj = hitInfo.collider.gameObject;
            return obj != player;
        }
        return false;
    }

    bool CanSeePlayer()
    {
        if(GetDistanceToPlayer() < visionRange)
        {
            return true;
        }
        return false;
    }

    void LookAtTarget()
    {
        oldRotation = transform.forward;
        Vector3 lookAt = playerposition;
        lookAt.y = transform.position.y;

        Vector3 lookDir = (lookAt - transform.position).normalized;
        // transform.forward = lookDir;
        targetRotation = lookDir;
        rotateTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        playerposition = player.transform.position;

        switch(state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Alert:
                Alert();
                break;
        }
    }

    void Idle()
    {
        if(CanSeePlayer())
        {
            state = State.Alert;
        }

        stateIndicator.text = "Idle...";
        transform.forward = Vector3.forward;
    }

    void Alert()
    {
        if (!CanSeePlayer())
        {
            state = State.Idle;
        }

        stateIndicator.text = "Alert!";
        transform.LookAt(player.transform);
    }

    enum State
    {
        Idle,
        Alert
    }
}
