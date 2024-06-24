using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Asteroids.HostSimple
{
    // 오브젝트 풀을 관리하기 위한 클래스?
    public class NetworkObjectPoolDefault : NetworkObjectProviderDefault
    {
        // 풀에 들어갈 오브젝트들, 모든 네트워크 오브젝트를 풀에서 관리한다면 비워두면 된다
        [Tooltip("The objects to be pooled, leave it empty to pool every Network Object spawned")] [SerializeField]
        private List<NetworkObject> _poolableObjects;

        // 생성되었다가 삭제된 오브젝트들이 들어있는 곳(재활용 목적)
        private Dictionary<NetworkObjectTypeId, Stack<NetworkObject>> _free = new();

        // 프리팹 생성하는 함수
        protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
        {
            if (ShouldPool(runner, prefab)) // 풀에서 관리할 오브젝트인지 확인
            {
                var instance = GetObjectFromPool(prefab);   // 풀에서 하나 꺼내기

                instance.transform.position = Vector3.zero; // 위치 초기화하기

                return instance;                            // 리턴
            }

            return Instantiate(prefab);     // 풀에서 관리하지 않는 오브젝트는 바로 생성해서 리턴
        }

        // 생성한 프리팹을 삭제하는 함수
        protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
        {
            if (_free.TryGetValue(prefabId, out var stack)) // 풀에서 관리하는 오브젝트인지 확인
            {
                instance.gameObject.SetActive(false);   // 비활성화 시키고
                stack.Push(instance);                   // 풀에 되돌리기
            }
            else
            {
                Destroy(instance.gameObject);           // 관리안하는 오브젝트는 그냥 삭제
            }
        }

        /// <summary>
        /// 풀에서 오브젝트 꺼내주는 함수
        /// </summary>
        /// <param name="prefab">꺼낼 네트워크 오브젝트</param>
        /// <returns>꺼낸 오브젝트</returns>
        private NetworkObject GetObjectFromPool(NetworkObject prefab)
        {
            NetworkObject instance = null;

            if (_free.TryGetValue(prefab.NetworkTypeId, out var stack)) // 네트워크 아이디로 풀스택 가져오기
            {
                while (stack.Count > 0 && instance == null)
                {
                    instance = stack.Pop();  // 스택에서 하나 꺼내기
                }
            }

            if (instance == null)
                instance = GetNewInstance(prefab);  // 스택에서 없으니 하나 새로 만들기

            instance.gameObject.SetActive(true);    // 활성화 시키고
            return instance;                        // 리턴
        }

        /// <summary>
        /// 풀이 바닥나거나 없을 때 오브젝트를 새로 만드는 함수
        /// </summary>
        /// <param name="prefab">만들 오브젝트</param>
        /// <returns>만들어진 오브젝트</returns>
        private NetworkObject GetNewInstance(NetworkObject prefab)
        {
            NetworkObject instance = Instantiate(prefab);   // 우선 생성

            if (_free.TryGetValue(prefab.NetworkTypeId, out var stack) == false)
            {
                stack = new Stack<NetworkObject>();         // 기존 스택이 없으면 새로 스택 생성
                _free.Add(prefab.NetworkTypeId, stack);     // 스택을 딕셔너리에 추가
            }

            return instance;    // 생성한 것 리턴
        }

        /// <summary>
        /// 풀에서 관리되는 오브젝트 인지 확인하는 함수
        /// </summary>
        /// <param name="runner">사용안함</param>
        /// <param name="prefab">네트워크 오브젝트인 프리팹</param>
        /// <returns>true면 풀에서 관리할 오브젝트, false면 일반 오브젝트</returns>
        private bool ShouldPool(NetworkRunner runner, NetworkObject prefab)
        {
            if (_poolableObjects.Count == 0)    // 풀러블 오브젝트에 들어있는게 없으면 무조건 true
            {
                return true;    // _poolableObjects에 등록된 오브젝트가 없으면 모두 풀에서 관리
            }

            return IsPoolableObject(prefab);
        }

        /// <summary>
        /// _poolableObjects에 등록된 오브젝트인지 확인하는 함수
        /// </summary>
        /// <param name="networkObject">확인할 오브젝트</param>
        /// <returns>_poolableObjects에 등록된 오브젝트이면 true, 아니면 false</returns>
        private bool IsPoolableObject(NetworkObject networkObject)
        {
            foreach (var poolableObject in _poolableObjects)
            {
                if (networkObject == poolableObject)
                {
                    return true;
                }
            }

            return false;
        }
    }
}