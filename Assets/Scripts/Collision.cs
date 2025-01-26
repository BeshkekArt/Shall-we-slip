using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]

    public Rigidbody2D rb;

    public bool onGround; //#1
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    private bool isOnSlope;
    public int wallSide;

    private float slopeDownAngle;
    private float slopeDownAngleOld;
    public float slopeAngle { get; private set;}
    public Vector2 slopeDirection { get; private set; }
    private Vector2 newVelocity;

    public CapsuleCollider2D capsuleCollider2D;
    private Vector2 colliderSize;
    private Vector2 colliderOffset;
    private Vector2 slopeNormalPerp;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    void Update()   
    {  
        onGround = Physics2D.OverlapCircle(colliderOffset + new Vector2(0.0f, -colliderSize.y), collisionRadius, groundLayer); //#1
        onWall = Physics2D.OverlapCircle(colliderOffset + new Vector2(colliderSize.x, 0.0f), collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle(colliderOffset + new Vector2(-colliderSize.x, 0.0f), collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle(colliderOffset + new Vector2(colliderSize.x, 0.0f), collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle(colliderOffset + new Vector2(-colliderSize.x, 0.0f), collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void FixedUpdate()
    {
        colliderSize = capsuleCollider2D.size / 2;  
        colliderOffset = capsuleCollider2D.offset + (Vector2)transform.position;
        SlopeVertical();
    }

    public void ApplySlope(float xInput, float movementSpeed)
    {
        newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
        rb.velocity = newVelocity;

        if (onGround && !isOnSlope)
        {
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.velocity = newVelocity;
        }

        else if (onGround && isOnSlope)
        {
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            rb.velocity = newVelocity;

        }

        else if (!onGround)
        {
            newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }


    private void SlopeVertical()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(colliderOffset + new Vector2(0.0f, -colliderSize.y), collisionRadius);
        Gizmos.DrawWireSphere(colliderOffset + new Vector2(colliderSize.x, 0.0f), collisionRadius);
        Gizmos.DrawWireSphere(colliderOffset + new Vector2(-colliderSize.x, 0.0f), collisionRadius);

        Gizmos.color = Color.yellow; // Измените цвет на любой удобный
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f); // Пример рейкаста вниз
    }
}
