using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Test_NavMesh : MonoBehaviour
{
    NavMeshAgent agent;
    TestInputActions inputActions;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        inputActions = new TestInputActions();

    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.LClick.performed += OnLClick;
        inputActions.Test.RClick.performed += OnRClick;
    }


    private void OnDisable()
    {
        inputActions.Test.RClick.performed -= OnRClick;
        inputActions.Test.LClick.performed -= OnLClick;
        inputActions.Test.Disable();
    }

    private void OnLClick(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);
        if( Physics.Raycast(ray, out RaycastHit hitInfo) )
        {
            agent.SetDestination(hitInfo.point);
        }
    }


    private void OnRClick(InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            //agent.SetDestination(hitInfo.point);
            //transform.position = hitInfo.point;
            agent.Warp(hitInfo.point);
        }
    }
}
