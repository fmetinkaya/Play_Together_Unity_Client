using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RandomManager
{
    int RANDOM_SEED_STATIC;
    int randomSeed;

    public RandomManager(int randomSeedStatic)
    {
        RANDOM_SEED_STATIC = randomSeedStatic;
        randomSeed = randomSeedStatic;
    }
    public float RandomGenerate(float min, float max)
    {
        Random.InitState(randomSeed+=100);
        return Random.Range(min, max);
    }

    public void resetSeed()
    {
        randomSeed = RANDOM_SEED_STATIC;
    }
}
