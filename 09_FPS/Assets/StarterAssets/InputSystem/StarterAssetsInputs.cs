using Cinemachine;
using System;
using System.Collections;
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

		/// <summary>
		/// 플레이어를 따라다니는 카메라
		/// </summary>
		CinemachineVirtualCamera followCamera;

		/// <summary>
		/// 줌 입력이 있으 떄 실행되는 델리게이트(bool:확대될 때 true, 원상복귀될 때 false)
		/// </summary>
		public Action<bool> onZoom;

        private void Start()
        {
			followCamera = GameManager.Instance.FollowCamera;
        }

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
			bool isPress = !context.canceled;

            StopAllCoroutines();
			StartCoroutine(Zoom(isPress));
			onZoom?.Invoke(isPress);
        }


		/// <summary>
		/// ZoomIn/ZoomOut을 처리하는 코루틴
		/// </summary>
		/// <param name="zoomIn">true면 확대, false면 원상복구</param>
		/// <returns></returns>
        IEnumerator Zoom(bool zoomIn)
        {
            const float zoomFOV = 20.0f;
            const float normalFOV = 40.0f;
            const float zoomTime = 0.25f;

            float speed = (normalFOV - zoomFOV) / zoomTime;

            float fov = followCamera.m_Lens.FieldOfView;

			if(zoomIn)
			{
                while (fov > zoomFOV)
                {
                    fov -= Time.deltaTime * speed;
                    followCamera.m_Lens.FieldOfView = fov;
                    yield return null;
                }
                followCamera.m_Lens.FieldOfView = zoomFOV;
            }
			else
			{
                while (fov < normalFOV)
                {
                    fov += Time.deltaTime * speed;
                    followCamera.m_Lens.FieldOfView = fov;
                    yield return null;
                }
				followCamera.m_Lens.FieldOfView = normalFOV;
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