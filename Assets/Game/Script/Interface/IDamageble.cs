using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ChType
//{
//    Player,
//    Enemy
//}

public interface IDamageble
{
    //ChType GetType { get; }

    public void AddDamage(float damage, GameObject attackObj);
    //ここに何属性か入れてもいいかも
}
