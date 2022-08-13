using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionParser : MonoBehaviour
{
	[SerializeField] QuestionController questionController;
	[SerializeField] private string filePath;
	List<QuestionData> questionDatas = new List<QuestionData>();
	[SerializeField] private CSVParser csvParser;
	private void Awake()
	{
		if (questionDatas.Count != 0) return;
		csvParser.Parse(filePath, DataFromWebCallBack);
	}

	private void DataFromWebCallBack(List<List<string>> questions)
	{
		if (questions == null || questions.Count == 0)
		{
			Logger.LogError("Questions not found");
			throw new Exception("Questions not found");
		}
		foreach (var question in questions)
		{
			if (question.Count == 0) continue;
			if (!int.TryParse(question[1], out var result))
			{
				Logger.LogError("Failed to parse question answer");
				continue;
			}

			var answers = new string [question.Count - 2];

			if (result <= 0 || result > answers.Length)
			{
				Logger.LogError("Failed to parse question answer");
				continue;
			}

			for (var i = 0; i < question.Count - 2; i++)
			{
				answers[i] = (question[2 + i]);
			}

			questionDatas.Add(new QuestionData(question[0], answers, result));
		}

		questionController.SetQuestions(questionDatas);
	}
}