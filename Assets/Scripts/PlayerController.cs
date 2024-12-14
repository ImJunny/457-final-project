using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bezier curve control points
    public Transform startPoint; // Bottom-left of the semicircle
    public Transform controlPoint1; // First control point
    public Transform controlPoint2; // Second control point
    public Transform endPoint; // Bottom-right of the semicircle

    // Movement variables
    private float t = 0.5f; // Initial position on the curve (0 = start, 1 = end)
    public float moveSpeed = 1f; // Speed of movement along the curve

    void Update()
    {
        // Player input for controlling t
        if (Input.GetKey(KeyCode.A))
        {
            t -= moveSpeed * Time.deltaTime; // Move left
        }
        if (Input.GetKey(KeyCode.D))
        {
            t += moveSpeed * Time.deltaTime; // Move right
        }

        // Clamp t to stay within the curve bounds (0 to 1)
        t = Mathf.Clamp01(t);

        // Update the position on the Bezier curve
        transform.position = GetBezierPoint(t);

        // Rotate the rectangle to align with the curve's direction
        AlignWithCurve(t);
    }

    // Calculate a point on the Bezier curve based on t
    private Vector3 GetBezierPoint(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * startPoint.position; // (1 - t)^3 * P0
        point += 3 * uu * t * controlPoint1.position; // 3(1 - t)^2 * t * P1
        point += 3 * u * tt * controlPoint2.position; // 3(1 - t) * t^2 * P2
        point += ttt * endPoint.position; // t^3 * P3

        return point;
    }

    // Align the rectangle's rotation to the tangent of the curve
    private void AlignWithCurve(float t)
    {
        // Small increment for tangent calculation
        float delta = 0.01f;

        // Handle special cases at the ends of the curve
        if (t <= delta)
        {
            // Use the direction between startPoint and controlPoint1
            Vector3 direction = (controlPoint1.position - startPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            return;
        }
        else if (t >= 1 - delta)
        {
            // Use the direction between controlPoint2 and endPoint
            Vector3 direction = (endPoint.position - controlPoint2.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            return;
        }

        // For all other positions, calculate tangent using t and t + delta
        float nextT = Mathf.Clamp01(t + delta);

        Vector3 currentPos = GetBezierPoint(t);
        Vector3 nextPos = GetBezierPoint(nextT);

        Vector3 tangentDirection = (nextPos - currentPos).normalized;
        float tangentAngle = Mathf.Atan2(tangentDirection.y, tangentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, tangentAngle);
    }
}