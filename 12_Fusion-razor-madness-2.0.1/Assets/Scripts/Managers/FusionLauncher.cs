using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System;
using Fusion.Sockets;

public class FusionLauncher : MonoBehaviour
{
	private NetworkRunner _runner;
	private ConnectionStatus _status;

	public enum ConnectionStatus
	{
		Disconnected,
		Connecting,
		Failed,
		Connected,
		Loading,
		Loaded
	}

	public async void Launch(GameMode mode, string room,
		INetworkSceneManager sceneLoader)
	{
		SetConnectionStatus(ConnectionStatus.Connecting, "");

		DontDestroyOnLoad(gameObject);

		if (_runner == null)
			_runner = gameObject.AddComponent<NetworkRunner>();
		_runner.name = name;
		_runner.ProvideInput = mode != GameMode.Server;

		await _runner.StartGame(new StartGameArgs()
		{
			GameMode = mode,
			SessionName = room,
			SceneManager = sceneLoader
		});
	}

	public void SetConnectionStatus(ConnectionStatus status, string message)
	{
		_status = status;
	}
}
