using UnityEngine;
using System.Collections;
using System.Diagnostics;


namespace Utils
{
	public enum CurrentCameraState
	{
		CameraStateBack,
		CameraStateUp
	}

	public class CameraController : MonoBehaviour
	{
		public static CurrentCameraState currentCameraState;
		public float cameraChangingSpeed = 0.2f;

		public Transform CameraBackTransform;
		public Transform CameraTopTransform;

		#region Private

		private Vector3 previousCameraTransformPosition;
		private Vector3 requiredCameraTransformPosition;
		private Quaternion previosCameraRotation;
		private Quaternion requiredCameraRotation;

		private float cameraPositionJourneyLength;
		private float cameraRotationJourneyLength;
		private float cameraInterpolationStartTime;

		#endregion

		public CurrentCameraState CurrentCameraState {
			get
			{
				return currentCameraState;
			}
		}

		private void Update ()
		{

			transform.localPosition = Vector3.MoveTowards (transform.localPosition,
			                                               requiredCameraTransformPosition,
			                                               cameraChangingSpeed);
			transform.rotation = Quaternion.Slerp (transform.rotation,
			                                       requiredCameraRotation,
			                                       cameraChangingSpeed);
				
		
		}

		public void ToggleUI ()
		{
			switch (currentCameraState)
			{

			case CurrentCameraState.CameraStateBack:
				{
					currentCameraState = CurrentCameraState.CameraStateUp;

					this.requiredCameraTransformPosition = CameraTopTransform.localPosition;
					this.previousCameraTransformPosition = CameraBackTransform.localPosition;

					this.requiredCameraRotation = CameraTopTransform.rotation;
					this.previosCameraRotation = CameraBackTransform.rotation;

					break;
				}
			case CurrentCameraState.CameraStateUp:
				{
					currentCameraState = CurrentCameraState.CameraStateBack;

					this.requiredCameraTransformPosition = CameraBackTransform.localPosition;
					this.previousCameraTransformPosition = CameraTopTransform.localPosition;

					this.requiredCameraRotation = CameraBackTransform.rotation;
					this.previosCameraRotation = CameraTopTransform.rotation;

					break;
				}
			default:
				break;
			}
		}

     
		// Use this for initialization
		private void Start ()
		{
			currentCameraState = CurrentCameraState.CameraStateBack;
		}

      
	}
}

