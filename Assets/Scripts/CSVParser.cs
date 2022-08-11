using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class CSVParser
{
	public static List<List<string>> Parse(string path)
	{
		var rawData = Resources.Load<TextAsset>(path);
		var splitRawData = rawData.text.Split(',');
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
		return data;
	}
}