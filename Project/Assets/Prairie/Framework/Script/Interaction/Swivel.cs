using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Prairie/Interactions/Door Swivel")]
public class Swivel : PromptInteraction
{
	public bool openFromLeft = false;

	private Vector3 hinge;
	private Vector3 direction;
	private float rotateSpeed = 90.0f;
	private float targetAngle = 0;
	const float rotationAmount = 1.5f;
	private float localx;
	private float localy;
	private float localz;
	private bool closed = true;

	void Start()
	{
		hinge = this.transform.position;
		float amt = 0.6f;

		// assuming target is a cube transformed to look like a door
		// the largest side should be the height
		// the second largest side is width used to construct the pivot point
		List<float> dimensions = new List<float>();
		dimensions.Add(this.transform.localScale.x);
		dimensions.Add(this.transform.localScale.y);
		dimensions.Add(this.transform.localScale.z);
		dimensions.Sort();
		amt *= dimensions[1];

		// opening from left requires a pivot point opposite its counterpart
		// and a different direction
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
			return "Open/Close Door";
		}
	}
}
