using UnityEngine;
using System.Collections;

public class TumbleInteraction : Interaction
{
	/// <summary>
	/// Allows user to rotate object.
	/// </summary>
	public bool pickedUp = false;
	private Vector3 facing;
	private Vector3 localX;

	// When the user interacts with object, they invoke the ability to 
	// tumble the object with the I, J, K and L keys. Interacting
	// with the object again revokes this ability.

	protected void Update()
	{
		if (pickedUp)
		{

			facing = trigger.GetComponent<FirstPersonInteractor> ().getViewpoint ().transform.position - transform.position;
			localX = Vector3.Cross (facing, transform.up);

			if (Input.GetKey (KeyCode.L)) // right
			{
				transform.RotateRelativeToCamera (-10, 0);
			}
			else if (Input.GetKey (KeyCode.J)) // left
			{
				transform.RotateRelativeToCamera (10, 0);
			}
			else if (Input.GetKey (KeyCode.K)) // down
			{
				transform.RotateRelativeToCamera (0, 10);
				//transform.Rotate(Vector3.right * rotateSpeed, Space.Self);
				//transform.RotateAround(transform.position, localX, 100 * Time.deltaTime);
			}
			else if (Input.GetKey (KeyCode.I)) // up
			{
				transform.RotateRelativeToCamera (0, -10);
				//transform.Rotate(Vector3.left * rotateSpeed, Space.World);
				//transform.RotateAround(transform.position, localX, -100 * Time.deltaTime);
			}
			else if (Input.GetKey (KeyCode.B))
			{
				transform.rotation = Quaternion.LookRotation(localX);
			}
			else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
			{
				if (trigger.GetComponent<FirstPersonInteractor> () != null)
					trigger.GetComponent<FirstPersonInteractor> ().SetIsFrozen (false);
				pickedUp = false;
			}
		}
	}

	protected override void PerformAction() {
		pickedUp = true;
		if (trigger.GetComponent<FirstPersonInteractor> () != null)
			trigger.GetComponent<FirstPersonInteractor> ().SetIsFrozen (true);
	}
}
