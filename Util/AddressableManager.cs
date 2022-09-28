using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;
using System;

namespace lLCroweTool.QC
{
    public static class AddressableManager
    {
        //어드레서블 통합처리//몇몇개 처리때문에 안될거 같은데
        //이넘에이터는 ref out in 다 안됨
        //애매한데 인터페이스로 처리해서 각자 처리할수 있게하는 법말고는 흠

        public  static void GetAddresableObject<T>(AssetReference assetReference, Action<AsyncOperationHandle> action) where T : UnityEngine.Object
        {
            AsyncOperationHandle opHandle = assetReference.LoadAssetAsync<T>();            
            opHandle.Completed += action;
        }
        public static IEnumerator GetAddresableObject2<T>(AssetReference assetReference, Action<AsyncOperationHandle> action) where T : UnityEngine.Object
        {
            AsyncOperationHandle opHandle = assetReference.LoadAssetAsync<T>();
            yield return opHandle;


            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                //targetObject = opHandle.Result as T;
            }
            else
            {
                Debug.LogError("AssetReference failed to load.");
            }
        }

        //public static T Complete<T>(AsyncOperationHandle obj) where T : UnityEngine.Object
        //{
        //    if (obj.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        targetObject = obj.Result as T;
        //    }
        //    else
        //    {
        //        Debug.LogError("AssetReference failed to load.");
        //    }
        //}

    }
}