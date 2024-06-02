using UnityEngine;

[System.Serializable]
public static class ThrowHandler
{

    [Header("SETTINGS")]

    [SerializeField] private static Vector3[] _tabPoints = new Vector3[0];
    [SerializeField] private static float _sizePoints = 10;
    [SerializeField] private static float _distanceBetweenPoints = 0.1f;

    // -- Bloque la valeur assignée aux limite definie -- //
    public static float FixValue(float value, float valueMin, float valueMax)
    {
        if (value < valueMin) { value = valueMin; }
        if (value > valueMax) { value = valueMax; }
        return value;
    }

    public static float FixValue(float value,Vector2 limit)
    {
        return FixValue(value, limit.x, limit.y);
    }

    // ---- Gestion des points de trajectoire ---- //

    public static Vector3[] GetPoints(Vector2 origin, Vector2 direction, float launchForce, int numberPoints)
    {
        Vector3[] tabPoints = new Vector3[0];

        tabPoints = InitializePoints(origin, numberPoints);

        tabPoints = PointDeplacement(tabPoints, origin, direction, launchForce);

        return tabPoints;
    }

    // -- Deplace les points de la trajectoire en fonction de PointPosition et les cache si ils ne sont pas utiliser -- //
    private static Vector3[] PointDeplacement(Vector3[] tabPoints,Vector2 origin,Vector2 direction, float launchForce)
    {

        for (int i = 0; i < tabPoints.Length; i++)
        {
            // Defini la distance entre les points et leurs emplacement
            tabPoints[i] = PointPosition(origin, direction, launchForce, i * _distanceBetweenPoints);
        }

        return tabPoints;

    }

    // -- Permet de calculer la position d'un point par rapport a une direction -- //
    private static Vector2 PointPosition(Vector2 origin, Vector2 direction, float launchForce, float t)
    {
        // Utilise le calcul suivant : P = p1 + vel * t + (at^2) / 2
        return origin + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
    }

    private static Vector3[] InitializePoints(Vector2 origin,int numberPoints)
    {
        // initialise le tableau de point
        Vector3[] tabPoints = new Vector3[numberPoints];

        // -- Cree les points utiliser pour representer la trajectoire du saut -- //
        for (int i = 1; i < numberPoints; i++)
        {
            // Cree le nombre de points demander
            tabPoints[i] = origin;
        }

        return tabPoints;
    }

    public static void LaunchObject(GameObject target,Vector2 direction,float lanchForce) 
    {
        Rigidbody rigidbody = target.GetComponent<Rigidbody>();
        Rigidbody2D rigidbody2D = target.GetComponent<Rigidbody2D>();
        CharacterController characterController = target.GetComponent<CharacterController>();

        if (rigidbody != null)
        {
            rigidbody.velocity = direction.normalized * lanchForce;
        }
        else if (rigidbody2D != null)
        {
            rigidbody2D.velocity = direction.normalized * lanchForce;
        }
        else if (characterController != null)
        {
            characterController.Move(direction.normalized * lanchForce);
        }
    }

    public static void OnDrawGizmos(Vector3 origin, Vector2 direction, float launchForce)
    {
        Vector3[] tabPoints = GetPoints(origin,direction,launchForce,10);

        Vector3 lastPoint = Vector3.zero;

        foreach (var point in tabPoints)
        {
            Gizmos.DrawSphere(point,_sizePoints);
        }

        for (int i = 0; i < tabPoints.Length; i++)
        {
            if (i > 0) { lastPoint = tabPoints[i-1]; } else { continue; }

            Gizmos.DrawLine(lastPoint, tabPoints[i]);
        }
    }

}