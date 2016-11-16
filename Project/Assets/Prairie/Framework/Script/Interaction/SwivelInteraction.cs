using UnityEngine;
using System.Collections;

public class SwivelInteraction : Interaction
{
	public bool clockwise = true;
	private Vector3 hinge;
	private float rotateSpeed = 90.0f;
	private float targetAngle = 0;
	const float rotationAmount = 1.5f;
	private bool closed = true;

	void Start()
	{
		hinge = this.transform.position;
		float amt = this.transform.localScale.z/2;
		if (clockwise)
		{
			hinge += amt * Vector3.forward;	
		}
		else
		{
			hinge -= amt * Vector3.forward;	
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
			transform.RotateAround (hinge, Vector3.up, -rotationAmount);
			targetAngle -= rotationAmount;
		}
		else if (targetAngle < 0)
		{
			transform.RotateAround (hinge, Vector3.up, rotationAmount);
			targetAngle += rotationAmount;
		}
	}
}