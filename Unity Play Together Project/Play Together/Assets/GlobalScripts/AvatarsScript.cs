using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarsScript
{
    Sprite[] avatars;
    public Sprite GetAvatar(int avatarNo)
    {
        avatars = Resources.LoadAll<Sprite>("Avatars/Avatars");
        return avatars[avatarNo];
    }
    public Sprite[] GetAvatarList()
    {
        return avatars = Resources.LoadAll<Sprite>("Avatars/Avatars");
    }
}
