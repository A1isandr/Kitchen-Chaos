using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
	private const string Cut = "Cut";

	[SerializeField] private CuttingCounter containerCounter;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		containerCounter.OnCut += CuttingCounter_OnCut;
	}

	private void CuttingCounter_OnCut(object sender, EventArgs e)
	{
		animator.SetTrigger(Cut);
	}
}
