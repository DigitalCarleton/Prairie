using UnityEngine;
using System.Collections;

public class SwivelInteraction : Interaction
{
	public bool openFromLeft = false;

	private Vector3 hinge;
	private Vector3 direction;
	private float rotateSpeed = 90.0f;
	private float targetAngle = 0;
	const float rotationAmount = 1.5f;
	private bool closed = true;

	void Start()
	{
		hinge = this.transform.position;
		float amt = 0.7f * this.transform.localScale.z;
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
			targetAngle -= rotateSpeed;
		}
		else
		{
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

	override public string defaultPrompt {
		get {
			return "Open Door";
		}
	}
}
