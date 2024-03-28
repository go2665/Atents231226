using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Vector3 offset;
    Player player;
    public float smoothness = 3.0f;

    private void Start()
    {
        player = GameManager.Instance.Player;
        transform.position = player.transform.position + Vector3.up * 30;
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, smoothness * Time.deltaTime);
    }
}
