using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

public class LevelManager : NetworkSceneManagerDefault
{
	[HideInInspector]
	public FusionLauncher Launcher;
	[SerializeField] private LoadingManager _loadingManager;
	private Scene _loadedScene;

	public void ResetLoadedScene()
	{
		_loadingManager.ResetLastLevelsIndex();
		_loadedScene = default;
	}

	protected override IEnumerator LoadSceneCoroutine(SceneRef sceneRef, NetworkLoadSceneParameters sceneParams)
	{
		_loadingManager.StartLoadingScreen();
		GameManager.Instance.SetGameState(GameManager.GameState.Loading);
		Launcher.SetConnectionStatus(FusionLauncher.ConnectionStatus.Loading, "");
		yield return new WaitForSeconds(1.0f);
		yield return base.LoadSceneCoroutine(sceneRef, sceneParams);
		Launcher.SetConnectionStatus(FusionLauncher.ConnectionStatus.Loaded, "");
		yield return new WaitForSeconds(1f);
		_loadingManager.FinishLoadingScreen();

	}
}
