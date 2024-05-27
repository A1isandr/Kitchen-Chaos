using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
	[SerializeField] private BaseCounter baseCounter;
	[SerializeField] private List<GameObject> visualGameObjects;

	private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
	}

	private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
	{
		if (e.SelectedCounter == baseCounter)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void Show()
	{	
		if (visualGameObjects != null)
		{
			foreach (var obj in visualGameObjects)
			{
				obj.SetActive(true);
			}
		}
	}

	private void Hide()
	{
		if (visualGameObjects != null)
		{
			foreach (var obj in visualGameObjects)
			{
				obj.SetActive(false);
			}
		}
	}
}
