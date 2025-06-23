using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public class FollowingCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);
        [SerializeField] private float smoothSpeed = 5f;

        private Vector3 targetPos;
        private Vector3 currentPos;

        private void LateUpdate()
        {
            if(!target) return;

            currentPos = transform.position;
            targetPos = target.position + offset;

            transform.position = Vector3.Lerp(currentPos, targetPos, smoothSpeed * Time.deltaTime);
        }
    }
}