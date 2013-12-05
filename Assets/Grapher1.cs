using UnityEngine;




//[ExecuteInEditMode]

public class Grapher1 : MonoBehaviour
{




    public int numArms;  		// number of spiral arms
    public int numPPA;  			// number of particles per arm
    public float partSep;			// separation between particles
    public float turnDist;		// distance between spiral turns
    public float vertDist = .001f;			// vertical turn distance
    public float originOffset;	// size of hole in middle of spiral
    public float turnSpeed;		// speed that spiral rotates.
    public float fade;				// fade particles along the arms
    public float size;				// change particle size along arms
    public float theta;
    public float armSpacing;






  
    public int resolution = 2;

    private int currentResolution;
    private ParticleSystem.Particle[] points;

    void Start()
    {
        theta = 0;
        resolution = numArms * numPPA;
        CreatePoints();
    }

    private void CreatePoints()
    {

        //	if(resolution < 2){
        //		resolution = 2;
        //	}
     
        points = new ParticleSystem.Particle[resolution];
        float increment = 1f / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].color = new Color(x, 0f, 0f);
            points[i].size = 0.1f;
        }
        currentResolution = resolution;
    }

    void Update()
    {
        resolution = numArms * numPPA;

        if (currentResolution != resolution)
        {
            CreatePoints();
        }
        //FunctionDelegate f = functionDelegates[(int)function];
        resolution = numArms * numPPA;
        theta = 0;


        //for(int i = 0; i < resolution; i++){
        //Vector3 p = points[i].position;


        for (int j = 0; j < numArms; j++)
        {

            float r = 0;
            theta = 0;
            float armRotation = j * armSpacing;

            // Loop thru the particles for this arm and place them.
            for (int i = 0; i < numPPA; i++)
            {

                int pX = j * numPPA + i;

                if (pX > resolution)
                {
                    return;
                }

                Vector3 p = points[pX].position;




                // This is the equation for our spiral in polar coords....
                r = originOffset + turnDist * theta;

                // All particles are positioned wrt the local transform origin.
                Vector3 newPos = transform.localPosition;
                //Vector3 newPos = effectObject.position;

                // Convert to Cartesian coords...
                // x = a * t * cos(t);
                // y = a * t * sin(t);

                //newPos.x = newPos.x + r * Mathf.Cos(theta);
                //newPos.y = newPos.y + r * Mathf.Sin(theta);
                newPos.z = newPos.z + pX * vertDist;

                //x = a * cos(t - 24) * sqrt(abs(t - 24)) * sign(t - 24);
                //y = a * sin(t - 24) * sqrt(abs(t - 24));
                newPos.x = newPos.x + r * Mathf.Cos((theta - 24) * Mathf.Sqrt(Mathf.Abs(theta - 24)) * Mathf.Sign(theta - 24));
                newPos.y = newPos.y + r * Mathf.Sin((theta - 24) * Mathf.Sqrt(Mathf.Abs(theta - 24)));
                p.z = newPos.z;



                p.x = newPos.x * Mathf.Cos(armRotation) + newPos.y * Mathf.Sin(armRotation);
                p.y = newPos.x * Mathf.Sin(armRotation) + newPos.y * Mathf.Cos(armRotation);




                theta += partSep;



                //p.y = f(p.x);
                points[pX].position = p;

                // Set particle size to account for specified variation.
                points[i].size = size;

                Color c = points[pX].color;
                c.g = p.y;
                c.a = c.a - i * fade;

                points[pX].color = c;
            }



            Mesh mesh = GetComponent<MeshFilter>().mesh;

            Vector3[] vertices = new Vector3[points.Length];
               // mesh.vertices;
            int k = 0;
            while (k < vertices.Length){
            
                vertices[k] += points[k].position;
                k++;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
        





        particleSystem.SetParticles(points, points.Length);

    }

}