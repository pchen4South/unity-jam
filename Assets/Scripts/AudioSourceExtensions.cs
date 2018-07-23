using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions 
{
	public static IEnumerator FadeOutOver(this AudioSource audioSource, float duration)
	{
		var remainingTime = duration;
		var initialVolume = audioSource.volume;

		while (remainingTime > 0)
		{
			yield return null;
			audioSource.volume = Mathf.Lerp(0f, initialVolume, remainingTime / duration);
			remainingTime -= Time.deltaTime;
		}
		audioSource.Stop();
		audioSource.volume = initialVolume;
	}
}