using UnityEngine;

public class FixPosition : MonoBehaviour
{
    [Header("Axis Fix Toggles")]
    [Tooltip("When checked, X will be locked to Constant X every frame")]
    public bool fixX;
    [Tooltip("When checked, Y will be locked to Constant Y every frame")]
    public bool fixY;
    [Tooltip("When checked, Z will be locked to Constant Z every frame")]
    public bool fixZ;

    [Header("Constant Position Values")]
    [Tooltip("The X position to enforce when Fix X is checked")]
    public float constantX;
    [Tooltip("The Y position to enforce when Fix Y is checked")]
    public float constantY;
    [Tooltip("The Z position to enforce when Fix Z is checked")]
    public float constantZ;

    void LateUpdate()
    {
        Vector3 pos = transform.localPosition;
        if (fixX) pos.x = constantX;
        if (fixY) pos.y = constantY;
        if (fixZ) pos.z = constantZ;
        transform.localPosition= pos;
    }
}
