using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float Speed = 1f;
    public Transform CameraTarget;

    private Vector3 _offset = new Vector3(0, 5, 0);
    private float _step;

    private List<PlayerBehaviour> _spectatingList = new List<PlayerBehaviour>();
    private bool _spectating = false;

    /// <summary>
    /// Set player state as spectator.
    /// </summary>
    public void SetSpectating()
    {
        _spectating = true;
        _spectatingList = new List<PlayerBehaviour>(FindObjectsOfType<PlayerBehaviour>());
        CameraTarget = GetRandomSpectatingTarget();
    }

    /// <summary>
    /// Return a random and different than current player transform.
    /// </summary>
    /// <returns></returns>
    private Transform GetRandomSpectatingTarget()
    {
        if (_spectatingList.Count == 1)
        {
            return CameraTarget;
        }
        return _spectatingList.Find(x => x.transform.GetChild(0) != CameraTarget).transform.GetChild(0);
    }

    /// <summary>
    /// Return next or prev player transform.
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    private Transform GetNextOrPrevSpectatingTarget(int to)
    {
        int currentIndex = _spectatingList.IndexOf(CameraTarget.GetComponentInParent<PlayerBehaviour>());

        if (currentIndex + to >= _spectatingList.Count)
        {
            return _spectatingList[0].transform.GetChild(0);
        }
        else if (currentIndex + to < 0)
        {
            return _spectatingList[_spectatingList.Count - 1].transform.GetChild(0);
        }
        else
        {
            return _spectatingList[currentIndex+to].transform.GetChild(0);
        }
    }

    private void Update()
    {
        if (_spectating)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CameraTarget = GetNextOrPrevSpectatingTarget(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CameraTarget = GetNextOrPrevSpectatingTarget(-1);
            }
        }
    }

    private void LateUpdate()
    {
        if (CameraTarget == null)
        {
            return;
        }

        _step = Speed * Vector2.Distance(CameraTarget.position, transform.position) * Time.deltaTime;

        Vector2 pos = Vector2.MoveTowards(transform.position, CameraTarget.position + _offset, _step);
        transform.position = pos;
    }
}
