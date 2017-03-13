using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Toggle Component")]
public class ComponentToggleInteraction : PromptInteraction 
{
	public Behaviour[] targets = new Behaviour[0];

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < targets.Length; i++)
		{
			// Draw red line(s) between the object and the objects whose Behaviours it toggles
            if (targets[i] != null)
            {
                Gizmos.DrawLine(transform.position, targets[i].transform.position);
            }
			
		}
	}

	protected override void PerformAction ()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].enabled = !targets[i].enabled;
		}
	}

	override public string defaultPrompt {
		get {
			return "Toggle Something";
		}
	}
}
