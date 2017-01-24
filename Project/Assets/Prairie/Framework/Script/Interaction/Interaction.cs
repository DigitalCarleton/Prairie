using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Prompt))]
public abstract class Interaction : MonoBehaviour
{
	public bool repeatable = true;

	[HideInInspector]
    public GameObject rootInteractor;

	/// <summary>
	/// Trigger behavoir of this interaction.
	/// </summary>
	/// <param name="interactor">The game object which is the root invoker of this action. Typically a player character.</param>
	public void Interact (GameObject interactor)
	{
        this.rootInteractor = interactor;
		if (this.enabled) {
			PerformAction ();					// run the interaction
			if (!repeatable) {
				this.enabled = false;	// prevent future interactions
			}
		}
	}

	protected abstract void PerformAction ();

	/// <summary>
	/// Return a default prompt suitable for this interaction type, for pre-populating a Prompt object.
	/// Note that this value will only affect the editing user experience.
	/// Changing this during gameplay will have NO EFFECT on the prompt.
	/// </summary>
	/// <value>A sensible default, or null if no such sensible default is appropriate.</value>
	public virtual string defaultPrompt {
		get { return null; }
	}

	// Typically, when a component is added to the inspector for the first time,
	// it automatically appends a 'Prompt' component as well.
	//
	// Having this call here ensures that, no matter the order of instantiation,
	// any newly added Prompt component recieves a default value.
	public void Reset()
	{
		Prompt prompt = this.gameObject.GetComponent<Prompt> ();
		if (prompt.promptText == null || prompt.promptText == "") {
			prompt.SetDefaultPrompt ();
		}
	}

}
