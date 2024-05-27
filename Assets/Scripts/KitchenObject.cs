using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

	private IKitcenObjectParent kitchenObjectParent;
	/// <summary>
	/// Represents the <see cref="KitchenObjectParent"/> that this <see cref="KitchenObject"/> is currently attached to.
	/// </summary>
	public IKitcenObjectParent KitchenObjectParent 
	{
		get => kitchenObjectParent;
		set
		{
			kitchenObjectParent?.ClearKitchenObject();

			kitchenObjectParent = value;

			if (value.HasKitchenObject())
			{
				Debug.LogError("Counter already has a KitchenObject!");
			}

			kitchenObjectParent.KitchenObject = this;

			transform.parent = value.GetKitchenObjectFollowTransform();
			transform.localPosition = Vector3.zero;
		}
	}

	/// <summary>
	/// Returns the <see cref="KitchenObjectSO"/> of this <see cref="KitchenObject"/>
	/// </summary>
	/// <returns></returns>
	public KitchenObjectSO GetKitchenObjectSO()
	{
		return kitchenObjectSO;
	}

	public void DestroySelf()
	{
		kitchenObjectParent.ClearKitchenObject();
		Destroy(gameObject);
	}

	public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitcenObjectParent kitchenObjectParent)
	{
		Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
		var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
		kitchenObject.KitchenObjectParent = kitchenObjectParent;

		return kitchenObject;
	}
}
