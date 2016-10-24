using UnityEngine;

public static class PlayerExtensions {

	/// <summary>
	/// Sets the player state to be locked if true, free to move if untrue.
	/// </summary>
	/// <param name="isFrozen">If <c>true</c>, the player can not move.</param>
	public static void SetIsFrozen(this FirstPersonInteractor player, bool isFrozen) {
		player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = !isFrozen;
		player.enabled = !isFrozen;
	}

}

public static class InteractionExtensions {

	/// <summary>
	/// Sets the player who triggered this Interaction's state to be locked if true, free to move if untrue.
	/// </summary>
	/// <param name="isFrozen">If <c>true</c>, the player can not move.</param>
	public static void SetPlayerIsFrozen(this Interaction i, bool isFrozen) {
		if (i.trigger != null) {
			var player = i.trigger.GetComponent<FirstPersonInteractor> ();
			if (player != null) {
				player.SetIsFrozen(isFrozen);
			}
		}
	}

}

public static class GameObjectExtensions {

	/// <summary>
	/// Interact with all Interactions attached to this GameObject.
	/// </summary>
	/// <param name="interactor">The invoker of the interaction, typically a player.</param>
	public static void InteractAll(this GameObject go, GameObject interactor) {
		foreach (Interaction i in go.GetComponents<Interaction> ()) {
			i.Interact (interactor);
		}
	}

}