using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitcenObjectParent
{
	[SerializeField] private Transform counterTopPoint;

	public KitchenObject KitchenObject { get; set; }

	/// <summary>
	/// Interaction logic for the counter.
	/// </summary>
	/// <param name="player"></param>
	public abstract void Interact(Player player);

	/// <summary>
	/// Alternate interaction logic for the counter.
	/// </summary>
	/// <param name="player"></param>
	public virtual void InteractAlternate(Player player)
	{
		Debug.Log("Alternate action has been called");
	}

	public Transform GetKitchenObjectFollowTransform()
	{
		return counterTopPoint;
	}

	public void ClearKitchenObject()
	{
		KitchenObject = null;
	}

	public bool HasKitchenObject()
	{
		return KitchenObject != null;
	}
}