using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	[SerializeField] private SoundType soundType;
	[SerializeField] private GameObject fillArea;
	private Slider slider;

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	private void Start()
	{
		slider.onValueChanged.AddListener(SetVolume);
	}

	private void SetVolume(float value)
	{
		SoundManager.instance.SetVolume(soundType, value);

		if (value <= 0.0001f)
			fillArea.SetActive(false);
		else
			fillArea.SetActive(true);
	}
}
