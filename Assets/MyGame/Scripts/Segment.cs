using System.Collections;
using UnityEngine;

public class Segment : MonoBehaviour
{
    private const float rot_duration = 0.4f;
    private static readonly Quaternion H_rotation = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion V_rotation = Quaternion.Euler(0, 0, 90);
    private static readonly Vector3 H_axis = Vector3.right;
    private static readonly Vector3 V_axis = Vector3.forward;

    public Transform[] segments;

    private readonly int[][] digitMap =
    {
        new[] {1, 1, 1, 1, 1, 1, 0}, // 0
        new[] {0, 1, 1, 0, 0, 0, 0}, // 1
        new[] {1, 1, 0, 1, 1, 0, 1}, // 2
        new[] {1, 1, 1, 1, 0, 0, 1}, // 3
        new[] {0, 1, 1, 0, 0, 1, 1}, // 4
        new[] {1, 0, 1, 1, 0, 1, 1}, // 5
        new[] {1, 0, 1, 1, 1, 1, 1}, // 6
        new[] {1, 1, 1, 0, 0, 0, 0}, // 7
        new[] {1, 1, 1, 1, 1, 1, 1}, // 8
        new[] {1, 1, 1, 1, 0, 1, 1}  // 9
    };

    private void Start()
    {
        InitializeSegments();
    }

    private void Update()
    {
        CheckForNumberInput();
    }

    private void InitializeSegments()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].rotation = IsHorizontalSegment(i) ? H_rotation : V_rotation;
        }
    }

    private void CheckForNumberInput()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (IsKeyPressed(i))
            {
                SetNumber(i);
            }
        }
    }

    private bool IsKeyPressed(int number) =>
        Input.GetKeyDown(number.ToString()) || Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + number));

    public void SetNumber(int number)
    {
        for (int i = 0; i < segments.Length; i++)
        {
            bool active = digitMap[number][i] == 1;
            Vector3 rotationAxis = IsHorizontalSegment(i) ? H_axis : V_axis;
            StartCoroutine(RotateSegment(segments[i], active, rotationAxis));
        }
    }

    private bool IsHorizontalSegment(int index) => index == 0 || index == 3 || index == 6;

    private IEnumerator RotateSegment(Transform segment, bool active, Vector3 axis)
    {
        Quaternion startRotation = segment.rotation;
        Quaternion targetRotation = GetTargetRotation(active, axis);

        float time = 0;

        while (time < rot_duration)
        {
            time += Time.deltaTime;
            segment.rotation = Quaternion.Lerp(startRotation, targetRotation, time / rot_duration);
            yield return null;
        }

        segment.rotation = targetRotation;
    }

    private Quaternion GetTargetRotation(bool active, Vector3 axis)
    {
        if (axis == H_axis)
        {
            return active ? H_rotation : Quaternion.Euler(-90, 0, 0);
        }
        return active ? V_rotation : Quaternion.Euler(0, 90, 90);
    }
}
