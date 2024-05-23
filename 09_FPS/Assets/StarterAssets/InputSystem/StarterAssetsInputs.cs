using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        public void OnMove(InputAction.CallbackContext context)
        {
			MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cursorInputForLook)
            {
                LookInput(context.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
			JumpInput(context.performed);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
			SprintInput(context.ReadValue<float>() > 0.1f);
        }

		public void OnZoom(InputAction.CallbackContext context)
		{
			// GameManager.Instance.FollowCamera.m_Lens.FieldOfView;

			const float zoomFOV = 20.0f;
			const float normalFOV = 40.0f;
			const float zoomTime = 0.25f;

			if (true)
			{
                // 마우스 오른 클릭을 눌렀을 때
                // zoomFOV까지 FollowCamera의 FOV가 줄어든다.
				// 총이 보이지 않아야 한다.
            }
            else
			{
                // 마우스 오른 클릭을 땠을 때
                // normalFOV까지 FollowCamera의 FOV가 늘어난다.
				// 총이 보여야 한다.
            }
        }

		public void OnFire(InputAction.CallbackContext context)
		{

		}

        public void OnReload(InputAction.CallbackContext context)
        {

        }


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}