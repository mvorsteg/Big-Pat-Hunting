using UnityEngine;

public class DebugCommands : MonoBehaviour
{
    public static void KillPlayer()
    {
        Player player = FindObjectOfType<Player>();
        player.TakeDamage(FallDamage.CalculateHit(player, -1000));
    }

    public static void KillAnimals()
    {
        Animal[] animals = FindObjectsOfType<Animal>();
        foreach (Animal animal in animals)
        {
            HitInfo info = new HitInfo(1000, 0, 0, Vector3.zero, FallDamage.instance);
            animal.TakeDamage(info);
        }
    }

}