using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
	[SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
	[SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

	public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
	public class OnStateChangedEventArgs : EventArgs 
	{
		public State state;
	}


	public enum State
	{
		Idle,
		Frying,
		Fried,
		Burned
	}

	private State state;
	private float fryingTimer;
	private float burningTimer;
	private FryingRecipeSO fryingRecipeSO;
	private BurningRecipeSO burningRecipeSO;

	private void Start()
	{
		state = State.Idle;
	}

	private void Update()
	{
		if (!HasKitchenObject()) return;

		switch (state)
		{
			case State.Idle:
				break;
			case State.Frying:
				fryingTimer += Time.deltaTime;

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });

				if (fryingTimer > fryingRecipeSO.fryingTimerMax)
				{
					KitchenObject.DestroySelf();

					KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

					burningRecipeSO = GetBurningRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
					state = State.Fried;
					burningTimer = 0f;

					OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
				}

				break;
			case State.Fried:
				burningTimer += Time.deltaTime;

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });

				if (burningTimer > burningRecipeSO.burningTimerMax)
				{
					KitchenObject.DestroySelf();

					KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

					state = State.Burned;

					OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f});
				}

				break;
			case State.Burned:
				break;
		}
	}

	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			if (player.HasKitchenObject() && HasRecipeWithInput(player.KitchenObject.GetKitchenObjectSO()))
			{
				player.KitchenObject.KitchenObjectParent = this;
		
				fryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
				
				state = State.Frying;
				fryingTimer = 0f;

				OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
			}
		}
		else
		{
			if (!player.HasKitchenObject())
			{
				KitchenObject.KitchenObjectParent = player;

				state = State.Idle;

				OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f});
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
		return GetFryingRecipeSOWithInput(inputKitchenObjectSO)?.output;
	}

	/// <summary>
	/// Checks if the given <see cref="KitchenObjectSO"/> has a recipe.
	/// </summary>
	/// <param name="inputKitchenObjectSO"></param>
	/// <returns></returns>
	private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		return fryingRecipeSOArray.Any(fryingRecipeSO => fryingRecipeSO.input == inputKitchenObjectSO);
	}

	/// <summary>
	/// Gets the frying <see cref="FryingRecipeSO"/> for the given <see cref="KitchenObjectSO"/>.
	/// </summary>
	/// <param name="inputKitchenObjectSO"></param>
	/// <returns></returns>
	[CanBeNull]
	private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		return fryingRecipeSOArray.FirstOrDefault(fryingRecipe => fryingRecipe.input == inputKitchenObjectSO);
	}

	/// <summary>
	/// Gets the burning <see cref="BurningRecipeSO"/> for the given <see cref="KitchenObjectSO"/>.
	/// </summary>
	/// <param name="inputKitchenObjectSO"></param>
	/// <returns></returns>
	[CanBeNull]
	private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		return burningRecipeSOArray.FirstOrDefault(burningRecipe => burningRecipe.input == inputKitchenObjectSO);
	}
}
