using UnityEngine;
using System.Collections;

public class TumbleInteraction : Interaction
{
	/// <summary>
	/// Allows user to rotate object.
	/// </summary>
	float rotateSpeed = 2f;
	public bool pickedUp = false;

	// When the user interacts with object, they invoke the ability to 
	// tumble the object with the I, J, K and L keys. Interacting
	// with the object again revokes this ability.
	protected override void PerformAction() {
		pickedUp = true;
		if (trigger.GetComponent<FirstPersonInteractor> () != null)
			trigger.GetComponent<FirstPersonInteractor> ().SetIsFrozen (true);
	}
		 
	protected void Update()
	{
		if (pickedUp)
		{
			Vector3 facing = trigger.GetComponent<FirstPersonInteractor> ().getViewpoint ().transform.position - transform.position;
			Vector3 localX = Vector3.Cross (facing, transform.up);
			if (Input.GetKey (KeyCode.L)) // left
			{
				transform.Rotate(Vector3.down * rotateSpeed, Space.World);
			}
			else if (Input.GetKey (KeyCode.J)) // right
			{
				transform.Rotate(Vector3.up * rotateSpeed, Space.World);
			}
			else if (Input.GetKey (KeyCode.K)) // down
			{
				//transform.Rotate(Vector3.right * rotateSpeed, Space.World);
				transform.RotateAround(transform.position, localX * rotateSpeed, 100 * Time.deltaTime);
			}
			else if (Input.GetKey (KeyCode.I)) // up
			{
				transform.Rotate(Vector3.left * rotateSpeed, Space.World);
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
}
