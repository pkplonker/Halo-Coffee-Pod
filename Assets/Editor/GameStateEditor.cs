//
// Copyright (C) 2022 Stuart Heath. All rights reserved.
//

using UnityEngine;
using UnityEditor;

/// <summary>
///GameStateEditor full description
/// </summary>
public static class GameStateEditor
{
	[MenuItem("Halo/ForceWin")]
	public static void ForceWin()
	{
		if (Application.isPlaying&&GameManager.instance != null)
		{
			GameManager.instance.OnWinDebug();
		}
	}
}