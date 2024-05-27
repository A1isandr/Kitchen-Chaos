using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitcenObjectParent
{
	/// <summary>
	/// Kitchen object.
	/// </summary>
	public KitchenObject KitchenObject { get; set; }

	/// <summary>
	/// Returns the follow transform of the counter.
	/// </summary>
	/// <returns></returns>
	public Transform GetKitchenObjectFollowTransform();

	/// <summary>
	/// Clears the kitchen object.
	/// </summary>
	public void ClearKitchenObject();

	/// <summary>
	/// Checks if the counter has a kitchen object.
	/// </summary>
	/// <returns></returns>
	public bool HasKitchenObject();
}
