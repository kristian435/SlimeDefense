using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCam : MonoBehaviour
{
    [Header("Liikkumis asetukset")]
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float scrollSpeed = 500f;
    [SerializeField] private float minY = 1f;
    [SerializeField] private float maxY = 40f;
    [Header("Hiiren asetus")]
    [SerializeField] private float mouseSens = 2f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;
    }
    private void Update()
    {
        Keyboard kb = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (kb == null ) return;

        float horizontal = 0f;
        float vertical = 0f;
        float upward = 0f;

        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) vertical += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) vertical -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) horizontal += 1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) horizontal -= 1f;
        if (kb.spaceKey.isPressed) upward += 1f;
        if (kb.leftCtrlKey.isPressed || kb.qKey.isPressed) upward -= 1f;

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = (forward * vertical +  right * horizontal + Vector3.up * upward).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (mouse != null && mouse.rightButton.isPressed)
        {
            Vector2 mouseDelta = mouse.delta.ReadValue();
            rotationX += mouseDelta.x * mouseSens * 0.1f;
            rotationY -= mouseDelta.y * mouseSens * 0.1f;
            rotationY = Mathf.Clamp(rotationY, 15f, 75f);
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }

        if (mouse != null )
        {
            float scroll = mouse.scroll.y.ReadValue();
            if (scroll != 0f)
            {
                float scrollAmount = scroll * 0.001f * scrollSpeed;

                transform.position += transform.forward * scrollAmount;

                Vector3 pos = transform.position;
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                transform.position = pos;
            }
        }
    }
}
