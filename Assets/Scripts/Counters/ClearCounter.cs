using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

	/// <summary>
	/// Handles the interaction with the counter
	/// </summary>
	public override void Interact(Player player)
    {
	    if (!HasKitchenObject())
	    {
		    if (player.HasKitchenObject())
		    {
			    player.KitchenObject.KitchenObjectParent = this;
		    }
	    }
	    else
	    {
		    if (!player.HasKitchenObject())
		    {
				KitchenObject.KitchenObjectParent = player;
			}
	    }
	}
}


