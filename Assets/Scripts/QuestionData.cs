using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Question")]
public class QuestionData : ScriptableObject
{
	public string question;
	public string[] answers;
	public int correctAnswer;
}