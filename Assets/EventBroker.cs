using System;

using UnityEngine;
public class EventBroker
{
    public static event Action EnemyWaveDefeated;
    public static void callEnemyWaveDefeated()
    {
        if (EnemyWaveDefeated != null)
            EnemyWaveDefeated();
    }


    // public static event Action MoveRight;
    // public static void CallMoveRight()
    // {
    //     if (MoveRight != null)
    //        { MoveRight();
    //        Debug.Log("Guys! Seriously! Move Right!!");
    //        }
    // }

    // public static event Action MoveLeft;
    // public static void CallMoveLeft()
    // {
    //     if (MoveLeft != null)
    //     {
    //         MoveLeft();
    //         Debug.Log("Guys! Seriously! Move Left!!");
    //     }
    // }
}
