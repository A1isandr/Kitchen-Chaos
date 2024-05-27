using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
	/// <summary>
	/// Reference to the prefab
	/// </summary>
	public Transform prefab;
	/// <summary>
	/// Reference to the sprite
	/// </summary>
	public Sprite sprite;
	/// <summary>
	/// Reference to the object name
	/// </summary>
	public string objectName;
}
