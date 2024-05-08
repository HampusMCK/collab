using UnityEngine;

public class Noise
{
    //0-20

    public delegate void OnNoise(Vector3 position, int strength);
   
    public static event OnNoise SoundEvent;

    //invoke the event with this void is to make sure that the event isn't invoked if it has no listeners
    public static void MakeNoise(Vector3 Position, int Intensity)
    {
        if (SoundEvent != null)
        {
            SoundEvent.Invoke(Position, Intensity);
        }
        else Debug.Log($"SoundEvent wasn't invoked, as there are no listeners");
    }
}