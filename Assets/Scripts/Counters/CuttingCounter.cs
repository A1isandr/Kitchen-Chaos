using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
	[SerializeField] private CuttingRecipeSO[] cutKitchenObjectSOArray;

	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
	public event EventHandler OnCut;

	private int cuttingProgress;

	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			if (player.HasKitchenObject() && HasRecipeWithInput(player.KitchenObject.GetKitchenObjectSO()))
			{
				player.KitchenObject.KitchenObjectParent = this;
				cuttingProgress = 0;

				var cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
				{
					progressNormalized = (float)cuttingProgress / cuttingRecipeSO!.cuttingProgressMax
				});
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

	public override void InteractAlternate(Player player)
	{
		if (HasKitchenObject() && HasRecipeWithInput(KitchenObject.GetKitchenObjectSO()))
		{
			cuttingProgress++;
			OnCut?.Invoke(this, EventArgs.Empty);

			var cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
			OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
			{
				progressNormalized = (float)cuttingProgress / cuttingRecipeSO!.cuttingProgressMax
			});

			if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
			{
				KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(KitchenObject.GetKitchenObjectSO());

				KitchenObject.DestroySelf();

				KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
			}
		}
	}

	/// <summary>
	/// Gets the output <see cref="KitchenObjectSO"/> for the given <see cref="KitchenObjectSO"/>.
	/// </summary>
	/// <param name="inputKitchenObjectSO"></param>
	/// <returns></returns>
	[CanBeNull]
	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
	{
		return GetCuttingRecipeSOWithInput(inputKitchenObjectSO)?.output;
	}

	/// <summary>
	/// Checks if the given <see cref="KitchenObjectSO"/> has a recipe.
	/// </summary>
	/// <param name="inputKitchenObjectSO"></param>
	/// <returns></returns>
	private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		return cutKitchenObjectSOArray.Any(cuttingRecipeSO => cuttingRecipeSO.input == inputKitchenObjectSO);
	}

	/// <summary>
	/// Gets the cutting <see cref="CuttingRecipeSO"/> for the given <see cref="KitchenObjectSO"/>.
	/// </summary>
	/// <param name="inputKitchenObjectSO"></param>
	/// <returns></returns>
	[CanBeNull]
	private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		return cutKitchenObjectSOArray.FirstOrDefault(cuttingRecipeSO => cuttingRecipeSO.input == inputKitchenObjectSO);
	}
}
