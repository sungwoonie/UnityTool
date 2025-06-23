using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public class NetworkHandler : SingleTon<NetworkHandler>
    {
        public float checkDelayTime;

        private WaitForSeconds checkTime;

        private event Action disconnectAction;
        private event Action connectAction;

        private NetworkReachability currentConnection;

        #region "Unity"

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        private void Start()
        {
            //set currentConnection to disconnected so can check internet disconnection
            currentConnection = NetworkReachability.ReachableViaCarrierDataNetwork;

            StartCoroutine(CheckNetworkConnection());
        }

        #endregion

        #region "Initialize"

        private void Initialize()
        {
            RegisterNetworkHandler(NetworkReachability.NotReachable, () => UIManager.instance.ShowNetworkIndicator(true, false));
        }

        #endregion

        #region "Handler"

        public void RegisterNetworkHandler(NetworkReachability networkType, Action action)
        {
            if(networkType == NetworkReachability.NotReachable)
            {
                disconnectAction += action;
            }
            else
            {
                connectAction += action;
            }
        }

        public void RemoveNetworkHandler(NetworkReachability networkType, Action action)
        {
            if(networkType == NetworkReachability.NotReachable)
            {
                disconnectAction -= action;
            }
            else
            {
                connectAction -= action;
            }
        }

        public void NetworkAction(NetworkReachability networkType)
        {
            currentConnection = networkType;

            DebugManager.DebugServerMessage($"Handle Network State. Current State : {currentConnection}");

            if(networkType == NetworkReachability.NotReachable)
            {
                if(disconnectAction != null)
                {
                    disconnectAction();
                }
            }
            else
            {
                if(connectAction != null)
                {
                    connectAction();
                    connectAction = null;
                }
            }
        }

        #endregion

        private IEnumerator CheckNetworkConnection()
        {
            if(checkTime == null)
            {
                checkTime = new WaitForSeconds(checkDelayTime);
            }

            while(true)
            {
                if(Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if(currentConnection != NetworkReachability.NotReachable)
                    {
                        NetworkAction(Application.internetReachability);
                    }
                }
                else
                {
                    if(currentConnection == NetworkReachability.NotReachable)
                    {
                        NetworkAction(Application.internetReachability);
                    }
                }

                yield return checkTime;
            }
        }
    }
}