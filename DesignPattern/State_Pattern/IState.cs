using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public interface IState<T>
    {
        void Handle(T controller);
        void StopFunction();
    }
}