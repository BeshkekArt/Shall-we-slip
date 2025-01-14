using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]

    public bool onGround; //#1
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    public float slopeAngle { get; private set;}
    public Vector2 slopeDirection { get; private set; }

    public CapsuleCollider2D capsuleCollider2D;
    private Vector2 colliderSize;
    private Vector2 colliderOffset;

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
        Slope();    
    }

    private void Slope()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);

        if (hit.collider != null)
        {
            Vector2 surfaceNormal = hit.normal;

            slopeAngle = Vector2.Angle(surfaceNormal, Vector2.up);

            if (slopeAngle > 0)
            {
                slopeDirection = new Vector2(surfaceNormal.y, -surfaceNormal.x).normalized;
            } else
              slopeDirection = Vector2.zero;
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
