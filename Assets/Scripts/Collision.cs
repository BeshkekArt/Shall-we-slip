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

    private CapsuleCollider2D capsuleCollider2D;
    private Vector2 colliderSize;


    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    void Start()
    {
        colliderSize = capsuleCollider2D.size;
    }

    void Update()
    {  
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + colliderSize, collisionRadius, groundLayer); //#1
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void FixedUpdate()
    {
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
                Debug.Log(slopeDirection);
            } else
              slopeDirection = Vector2.zero;
              Debug.Log(slopeDirection);
        } 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + colliderSize.y, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.color = Color.yellow; // Измените цвет на любой удобный
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f); // Пример рейкаста вниз
    }
}
