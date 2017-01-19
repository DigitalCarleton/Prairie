using UnityEngine;
using System.Collections;

public class SwivelInteraction : Interaction
{
	public bool openFromLeft = false;

	private Prompt prompt;
	private Vector3 hinge;
	private Vector3 direction;
	private float rotateSpeed = 90.0f;
	private float targetAngle = 0;
	const float rotationAmount = 1.5f;
	private bool closed = true;

	void Start()
	{
		prompt = this.GetComponent<Prompt>();
		prompt.promptText = "Click to Open Door";
		hinge = this.transform.position;
		float amt = this.transform.localScale.z/2;
		if (openFromLeft)
		{
			hinge += amt * Vector3.forward;
			direction = Vector3.up;
		}
		else
		{
			hinge -= amt * Vector3.forward;
			direction = Vector3.down;
		}
	}

	protected override void PerformAction()
	{
		if (closed)
		{
			prompt.promptText = "Click to Close Door";
			targetAngle -= rotateSpeed;
		}
		else
		{
			prompt.promptText = "Click to Open Door";
			targetAngle += rotateSpeed;	
		}
		closed = !closed;
	}

	void Update()
	{
		if (targetAngle != 0)
		{
			Rotate();
		}
	}

	protected void Rotate()
	{
		if (targetAngle > 0)
		{
			transform.RotateAround (hinge, direction, -rotationAmount);
			targetAngle -= rotationAmount;
		}
		else if (targetAngle < 0)
		{
			transform.RotateAround (hinge, direction, rotationAmount);
			targetAngle += rotationAmount;
		}
	}
}
