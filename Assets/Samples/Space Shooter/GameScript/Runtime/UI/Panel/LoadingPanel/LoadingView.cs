using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class LoadingView : ViewBase
{
    public override void Init(UIWindow uiBase)
    {
        base.Init(uiBase);
        GameObject play = YooAssets.LoadAssetSync<GameObject>("Player").AssetObject as GameObject;
        GameObject player = GameObject.Instantiate(play);
        player.AddComponent<PlayerManager>();
    }
}
