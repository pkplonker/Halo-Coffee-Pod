using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
	public static SFXPlayer instance;
	private PlayerMovement player;
	private AudioSource audioSource;
	private QuestionController questionController;
	[SerializeField] AudioClip winSound;
	[SerializeField] private AudioClip moveSound;
	[SerializeField] private AudioClip slideSound;
	[SerializeField] private AudioClip correctAnswerSound;
	[SerializeField] private AudioClip wrongAnswerSound;


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void RegisterGameManager(GameManager manager)
	{
		manager.OnNewGame += OnNewGame;
	}

	private void OnNewGame(PlayerMovement player, QuestionController questionController)
	{
		if (questionController)
		{
			this.questionController.OnCorrectAnswer -= OnCorrectAnswer;
			this.questionController.OnWrongAnswer -= OnWrongAnswer;
		}

		this.questionController = questionController;
		this.questionController.OnCorrectAnswer += OnCorrectAnswer;
		this.questionController.OnWrongAnswer += OnWrongAnswer;

		if (this.player != null)
		{
			this.player.OnMove -= OnMove;
			this.player.OnSlide -= OnSlide;
			this.player.OnWin -= OnWin;
			this.player.OnMoveComplete -= OnMoveComplete;
		}

		this.player = player;

		player.OnMove += OnMove;
		player.OnSlide += OnSlide;
		player.OnWin += OnWin;
		player.OnMoveComplete += OnMoveComplete;
	}

	private void PlayClip(AudioClip clip)
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
			if (audioSource == null) return;
		}

		if (clip == null) return;
		audioSource.PlayOneShot(clip);
	}

	private void OnCorrectAnswer(PlayerMovement obj)
	{
		PlayClip(correctAnswerSound);
	}

	private void OnWrongAnswer(PlayerMovement obj)
	{
		PlayClip(wrongAnswerSound);
	}

	private void OnSlide()
	{
		PlayClip(slideSound);
	}

	private void OnMoveComplete()
	{
		audioSource.Stop();
	}

	private void OnMove()
	{
		PlayClip(moveSound);
	}

	private void OnWin()
	{
		audioSource.Stop();
		PlayClip(winSound);
	}

	private void OnDisable()
	{
		GameManager.instance.OnNewGame -= OnNewGame;
		if (player == null) return;
		player.OnMove -= OnMove;
		player.OnSlide -= OnSlide;
		player.OnWin -= OnWin;
		player.OnMoveComplete -= OnMoveComplete;
	}
}