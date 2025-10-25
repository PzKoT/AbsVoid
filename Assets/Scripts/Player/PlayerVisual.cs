using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private const string IS_RUNNING = "IsRunning";
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (PauseMenu.IsPaused) return;

        if (Player.Instance == null || GameInput.Instance == null) return;

        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        if (Player.Instance == null || GameInput.Instance == null) return;

        Vector3 mousePos = GameInput.Instance.GetMouseWorldPosition();
        Vector3 playerPos = transform.position;

        spriteRenderer.flipX = mousePos.x < playerPos.x;
    }
}