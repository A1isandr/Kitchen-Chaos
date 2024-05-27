using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitcenObjectParent
{
	[SerializeField] private float movementSpeed = 7f;
	[SerializeField] private float rotateSpeed = 10f;
	[SerializeField] private float playerRadius = .7f;
	[SerializeField] private float playerHeight = 1.8f;
	[SerializeField] private float interactDistance = 2f;
	[SerializeField] private GameInput gameInput;
	[SerializeField] private LayerMask countersLayerMask;
	[SerializeField] private Transform kitchenObjectHoldPoint;

	public KitchenObject KitchenObject { get; set; }

	/// <summary>
	/// Fired when the selected counter changes
	/// </summary>
	public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
	/// <summary>
	/// Event args for <see cref="OnSelectedCounterChanged"/>
	/// </summary>
	public class OnSelectedCounterChangedEventArgs : EventArgs
	{
		public BaseCounter SelectedCounter;
	}

	/// <summary>
	/// Returns the singleton instance of the player
	/// </summary>
	public static Player Instance { get; private set; }
	/// <summary>
	/// Returns true if the player is walking
	/// </summary>
	public bool IsWalking { get; private set; }
	/// <summary>
	/// Returns the direction the player is moving
	/// </summary>
	public Vector3 MoveDirection 
	{ 
		get
		{
			Vector2 inputVector = gameInput.GetMovementVectorNormalized();
			return new Vector3(inputVector.x, 0f, inputVector.y);
		}
	}
	/// <summary>
	/// Returns the last direction the player interacted with
	/// </summary>
	public Vector3 LastInteractDirection { get; private set; }

	private BaseCounter selectedCounter;
	private BaseCounter SelectedCounter 
	{
		get => selectedCounter;
		set
		{
			selectedCounter = value;

			OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs()
			{
				SelectedCounter = SelectedCounter
			});
		}
	}

	private void Awake()
	{
		if (Instance != null)
		{
			throw new Exception("There is more than one Player instance");
		}

		Instance = this;
	}

	private void Start()
	{
		gameInput.OnInteractAction += GameInputOnInteractAction;
		gameInput.OnInteractAlternateAction += GameInputOnInteractAlternateAction;
	}

	private void Update()
	{
		HandleMovement();
		HandleInteractions();
	}

	private void GameInputOnInteractAction(object sender, EventArgs e)
	{
		if (SelectedCounter != null)
		{
			SelectedCounter.Interact(this);
		}
	}

	private void GameInputOnInteractAlternateAction(object sender, EventArgs e)
	{
		if (SelectedCounter != null)
		{
			SelectedCounter.InteractAlternate(this);
		}
	}

	private void HandleInteractions() 
	{
		Vector3 moveDirection = MoveDirection;

		if (moveDirection != Vector3.zero)
		{
			LastInteractDirection = moveDirection;
		}

		if (Physics.Raycast(transform.position, LastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
		{
			if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
			{
				// Has KitchenObjectParent
				if (baseCounter != SelectedCounter)
				{
					SelectedCounter = baseCounter;
				}
			}
			else
			{
				SelectedCounter = null;
			}
		}
		else
		{
			SelectedCounter = null;
		}
	}

	private void HandleMovement()
	{
		Vector3 moveDirection = MoveDirection;
		float moveDistance = movementSpeed * Time.deltaTime;
		bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

		// Cannot move in this direction
		if (!canMove)
		{
			// Attempt only X movement
			Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
			canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

			if (canMove)
			{
				// Can move only on the X
				moveDirection = moveDirectionX;
			}
			else
			{
				// Cannot move only on the X
				// Attempt only Z movement
				Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
				canMove = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

				if (canMove)
				{
					// Can move only on the Z
					moveDirection = moveDirectionZ;
				}
				else
				{
					// Cannot move in any direction
				}
			}
		}

		if (canMove)
		{
			transform.position += moveDistance * moveDirection;
		}

		IsWalking = moveDirection != Vector3.zero;

		transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
	}

	public Transform GetKitchenObjectFollowTransform()
	{
		return kitchenObjectHoldPoint;
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
