using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    private EntityManager entityManager;
    CharacterController characterController;


    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = entityManager.CreateEntity();

        GetComponent<PlayerInput>().onActionTriggered += HandleAction;
        characterController = GetComponent<CharacterController>();

        characterController.Move(new Vector3(50, 0, 50));
    }

    private void HandleAction(InputAction.CallbackContext context)
    {
        if(context.action.name == "Jump")
        {
            //HandleJump();
        }
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        Vector2 moveVec = new Vector3(inputVec.x, 0, inputVec.y);
        characterController.Move(moveVec);
    }
}
