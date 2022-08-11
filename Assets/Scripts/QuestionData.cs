using System;
using UnityEngine;

[Serializable]
public class QuestionData 
{
	public string question;
	public string[] answers;
	public int correctAnswer;
	public QuestionData(string question, string[] answers, int correctAnswer)
	{
		this.question = question;
		this.answers = answers;
		this.correctAnswer = correctAnswer;
	}
}