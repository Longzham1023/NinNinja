using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IdkeState : IState
{
    float randomTime;
    float timer;

    public void OnExecute(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0;
        randomTime = Random.Range(2f, 4f);
    }

    public void OnEnter(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public void OnExit(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }
}