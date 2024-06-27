using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Tooltip("释放者")] public Entity origin;

    [Tooltip("目标")] public Entity target;

    //在这个方法中组合技能效果
    //使用技能
    public abstract void SetOriginAndTarget(Entity origin, Entity target);
}
