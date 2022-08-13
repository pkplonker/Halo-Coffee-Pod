using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CSVParser : MonoBehaviour
{
	public void Parse(string path, Action<List<List<string>>> handbackCallback)
	{
		//	var rawData = Resources.Load<TextAsset>(path);
		path = Path.Combine(Application.streamingAssetsPath, path);

#if UNITY_EDITOR
		Logger.Log("Local --    " + path);
		StartCoroutine(GetDataFromLocal(path, ParseCallBack, handbackCallback));

#elif UNITY_WEBGL
		Logger.Log("Web --    " +path);
		StartCoroutine(GetDataFromWeb(path, ParseCallBack, handbackCallback));
#endif
	}

	private IEnumerator GetDataFromLocal(string path, Action<string, Action<List<List<string>>>> callback,
		Action<List<List<string>>> handbackCallback)
	{
		var rawData = "";
		if (File.Exists(path))
		{
			rawData = File.ReadAllText(path);
		}

//		Debug.Log(rawData);
		if (string.IsNullOrEmpty(rawData))
		{
			//	Logger.LogError("rawData is null");
			yield break;
		}

		callback(rawData, handbackCallback);
	}

	private IEnumerator GetDataFromWeb(string path, Action<string, Action<List<List<string>>>> callback,
		Action<List<List<string>>> handbackCallback)
	{
		Logger.Log("cor");

		UnityWebRequest www = UnityWebRequest.Get(path);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Logger.Log($"error msg = {www.error.ToString()}");
		}
		else
			callback?.Invoke(www.downloadHandler.text,handbackCallback);
	}


	public static void ParseCallBack(string rawData, Action<List<List<string>>> handbackCallback)
	{
		Logger.Log("parsecallback");

		var splitRawData = rawData.Split(',');
		var data = new List<List<string>>(splitRawData.Length);
		var currentRow = new List<string>();
		for (int i = 0; i < splitRawData.Length; i++)
		{
			var current = splitRawData[i].Split('\r', '\n');
			foreach (var c in current)
			{
				if (string.IsNullOrWhiteSpace(c))
				{
					data.Add(currentRow);
					currentRow = new List<string>();
				}
				else
				{
					currentRow.Add(c);
				}
			}
		}

		if (currentRow.Count != 0) data.Add(currentRow);
		data.RemoveAt(0);
		Logger.Log("data count " + data.Count);

		handbackCallback?.Invoke(data);
	}
}