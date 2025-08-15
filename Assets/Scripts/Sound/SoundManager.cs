using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
	Master,
	Gameplay,
	Lobby,
	SFX,
}

public enum GameplaySound
{
	Main
}

public enum LobbySound
{
	None
}


public enum SFXSound
{
	Booting, Rolling, Event
}


public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	[SerializeField] private AudioMixer mixer;
	[SerializeField] private AnimationCurve volumeCurve;

	//AudioClip 모음
	[SerializeField] private AudioClip[] gameplayClips;
	[SerializeField] private AudioClip[] lobbyClips;
	[SerializeField] private AudioClip[] sfxClips;

	//AudioSource 모음 AS = AudioSource의 준말
	[SerializeField] private AudioSource gameplayAS;
	[SerializeField] private AudioSource lobbyAS;
	[SerializeField] private AudioSource sfxAS;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
        
        PlayBGM(GameplaySound.Main); // 임시

    }
	private void Update()
	{

	}

	public void PlayBGM(GameplaySound gamePlaySound)
	{
		//if (lobbyAS.isPlaying) lobbyAS.Stop();
		gameplayAS.clip = gameplayClips[(int)gamePlaySound];
		gameplayAS.Play();
	}

	public void PlayBGM(LobbySound lobbySound)
	{
       /* if (gameplayAS.isPlaying) gameplayAS.Stop();
        lobbyAS.clip = lobbyClips[(int)lobbySound];
		lobbyAS.Play();*/
	}

	/*public void StopLobbyBGM()
	{
		lobbyAS.Stop();
	}*/

	public void StopGameBGM()
	{
		gameplayAS.Stop();
	}

	public void PlaySFX(SFXSound sfxSound)
	{
        sfxAS.PlayOneShot(sfxClips[(int)sfxSound]);
	}

	public void SetVolume(SoundType soundType, float volume)
	{
		float curvedVolume = volumeCurve.Evaluate(volume);
		float volumeInDb = Mathf.Log10(Mathf.Clamp(curvedVolume, 0.0001f, 1f)) * 20f;

		if (volume <= 0.0001f)
		{
			volumeInDb = -80f;
		}

		mixer.SetFloat(soundType.ToString(), volumeInDb);
	}

    public void StopGameBGMWithFade(float fadeDuration = 3f)
    {
        StartCoroutine(FadeOutBGM(fadeDuration));
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVolume = gameplayAS.volume;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            gameplayAS.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        gameplayAS.Stop();
        gameplayAS.volume = startVolume; // 다음 재생 대비 원상복구
    }
}
