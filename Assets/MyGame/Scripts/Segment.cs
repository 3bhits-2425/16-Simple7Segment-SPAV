using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Segment : MonoBehaviour
{
    public Transform[] segments; 

    private readonly int[][] digitMap = new int[][]
    {
        new int[] {1, 1, 1, 1, 1, 1, 0}, // 0
        new int[] {0, 1, 1, 0, 0, 0, 0}, // 1
        new int[] {1, 1, 0, 1, 1, 0, 1}, // 2
        new int[] {1, 1, 1, 1, 0, 0, 1}, // 3
        new int[] {0, 1, 1, 0, 0, 1, 1}, // 4
        new int[] {1, 0, 1, 1, 0, 1, 1}, // 5
        new int[] {1, 0, 1, 1, 1, 1, 1}, // 6
        new int[] {1, 1, 1, 0, 0, 0, 0}, // 7
        new int[] {1, 1, 1, 1, 1, 1, 1}, // 8
        new int[] {1, 1, 1, 1, 0, 1, 1}  // 9
    };

    private void Start()
    {
        
        for (int i = 0; i < segments.Length; i++)
        {
            if (i == 0 || i == 3 || i == 6)
            {
                segments[i].rotation = Quaternion.Euler(0, 0, 0);
            }
            else 
            {
                segments[i].rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) || Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + i)))
            {
                SetNumber(i);
            }
        }
    }

    public void SetNumber(int number)
    {
        

        for (int i = 0; i < segments.Length; i++)
        {
            bool active = digitMap[number][i] == 1;

            if (i == 0 || i == 3 || i == 6) 
            {
                StartCoroutine(RotateSegment(segments[i], active, Vector3.right));
            }
            else 
            {
                StartCoroutine(RotateSegment(segments[i], active, Vector3.forward));
            }
        }
    }

    private IEnumerator RotateSegment(Transform segment, bool active, Vector3 axis)
    {
        Quaternion startRotation = segment.rotation;
        Quaternion targetRotation;

        if (axis == Vector3.right) 
        {
            targetRotation = active ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(-90, 0, 0);
        }
        else 
        {
            targetRotation = active ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 90, 90);
        }

        float time = 0;
        float duration = 0.4f;

        while (time < duration)
        {
            time += Time.deltaTime;
            segment.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            yield return null;
        }
        segment.rotation = targetRotation;
    }
}

//IEnumerator RotateSegment(...) → Eine Coroutine, die ein Segment langsam dreht, indem sie die Drehung über mehrere Frames verteilt.

//StartCoroutine(RotateSegment(...)) → Startet die Coroutine, damit die Drehung nicht sofort, sondern flüssig über Zeit abläuft.

//Quaternion.Euler(x, y, z) → Erstellt eine Rotation in Unity anhand von drei Winkeln (x, y, z), um die Segmente in die richtige Position zu bringen.

//Quaternion.Lerp(startRotation, targetRotation, t) → Berechnet eine sanfte Drehung zwischen zwei Rotationen (startRotation → targetRotation) basierend auf t (Verhältnis der Animation).

//yield return null; → Wartet einen Frame, bevor die Coroutine weiterläuft, damit die Bewegung schrittweise passiert.