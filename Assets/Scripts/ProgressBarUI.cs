using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{
	[SerializeField] private GameObject hasProgressGameObject;
	[SerializeField] private Image barImage;

	private IHasProgress hasProgress;

	private void Start()
	{
		hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
		if (hasProgress is null)
		{
			Debug.LogError("Game object " + hasProgressGameObject + " does not have a component that implements IHasProgress");
		}

		hasProgress.OnProgressChanged += HasProgress_OnOnProgressChanged;
		barImage.fillAmount = 0f;
		Hide();
	}

	private void HasProgress_OnOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
	{
		barImage.fillAmount = e.progressNormalized;

		if (e.progressNormalized is 0f or 1f)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	private void Show()
	{
		gameObject.SetActive(true);
	}

	private void Hide()
	{
		gameObject.SetActive(false);
	}
}
