using UnityEngine;




[ExecuteInEditMode]

public class Grapher2 : MonoBehaviour
{
    public bool doUpdate = true;
    public bool alwaysDoUpdate = true;
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
    public double mod = 0.00000005f;
    private int resolution = 2;
    private int stepSize = 2;
    public bool[] patternbools;
    private bool[] _patternbools;
    private int patternIndex = 0;
    private int patternIndex2 = 0;

    public tk2dSpriteCollectionData spriteCollection;
    public bool boolupdate = true;
    public tk2dSpriteDefinition [] Sprites;
    public float rotationOffset; // Rotation offset in relation to the normal created by the point and the corresponding point on the ring above it
    public string selectedSpriteName;
    public float spriteScale;

    static Grapher2 instance;

    private int currentResolution;
    [SerializeField]
    public ParticleSystem.Particle[] points;
    public Particle[] allPoints;
    private bool MeshGen;

    void Start()
    {
        //theta = 0;
        //resolution = numArms * numPPA;
        //CreatePoints();
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

          //  Debug.DrawLine(points[i].position, new Vector3(points[i].position.x, points[i].position.y,100f));
            
            currentResolution = resolution;
        }

        
    }

  


    void Update()
    {

      //  _patternbools = patternbools;


        resolution = numArms * numPPA;
        if (resolution > 0)
        {
            points = new ParticleSystem.Particle[resolution];
        }
        else
        {
            points = new ParticleSystem.Particle[0];
        }
        if (spriteCollection)
        {
            spriteCollection = transform.parent.GetComponent<tk2dSprite>().Collection;
            selectedSpriteName = transform.parent.GetComponent<tk2dSprite>().CurrentSprite.name;

        }

        if (spriteCollection)
        {

            Sprites = spriteCollection.spriteDefinitions;

        }

        if (doUpdate || alwaysDoUpdate)
        {

            
       // spriteCollection = this.gameObject.GetComponent<tk2dSprite>().Collection;
       // selectedSpriteName = this.gameObject.GetComponent<tk2dSprite>().CurrentSprite.name;

           
            float increment = 1f / (resolution - 1);
            for (int i = 0; i < points.Length; i++)
            {
                float x = i * increment;

                points[i].position = new Vector3(x, 0f, 0f);
                points[i].color = new Color(x, 0f, 0f);
                points[i].size = 0.1f;
            }
            currentResolution = resolution;

            //particleSystem.Pause();
            resolution = numArms * numPPA;

          //  if (currentResolution != resolution)
          //  {
          //      CreatePoints();
          //  }
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
                for (int i = 1; i < numPPA; i++)
                {

                    int pX = j * numPPA + i;

                    if (pX > resolution)
                   {
                       return;
                    }

                    Vector3 p = points[pX].position;


                    ///////






                    //////

                    // This is the equation for our spiral in polar coords....
                    r = originOffset + turnDist * theta;

                    // All particles are positioned wrt the local transform origin.
                    Vector3 newPos =  Vector3.zero;// transform.position;
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




                    theta += partSep - ((float)mod * (i * 2));



                    //p.y = f(p.x);
                    points[pX].position = p;

                    // Set particle size to account for specified variation.
                    points[i].size = size;

                    Color c = points[pX].color;
                    c.g = p.y;
                    c.a = c.a - i * fade;

                    points[pX].color = c;
                }


                if (MeshGen)
                {
                    Mesh mesh = GetComponent<MeshFilter>().mesh;

                    Vector3[] vertices = new Vector3[points.Length];
                    // mesh.vertices;
                    int k = 0;
                    while (k < vertices.Length)
                    {

                        vertices[k] += points[k].position;
                        k++;
                    }

                    mesh.vertices = vertices;
                    mesh.RecalculateBounds();
                }
            }
       
          //  particleSystem.Play();
          
           // particleSystem.SetParticles(points, points.Length);
            // particleSystem.Pause();
            doUpdate = false;
 }
            //particleSystem.SetParticles(points, points.Length);
            

            //Place Sprites toggle 
            if (boolupdate)
            {
                boolupdate = false;
                place();


            }

           // particleSystem.Play();


    }



    int getPatternIndex()
    {
        
            patternIndex++;

            if (patternIndex == patternbools.Length)
            {
                patternIndex = 0;
            }

        return patternIndex;
    }
    




    void OnDrawGizmos()
    {
        patternIndex = 0;
        for (int i = 0; i < resolution; i++)
        {

            try
            {

                if (patternbools[getPatternIndex()])
                {
                    Gizmos.DrawCube(points[i].position + transform.position, new Vector3(1f, 1f, 1f));
                }
            }
            catch { }
        }

    }

   public void place()
    {

        patternIndex2 = 0;

        GameObject go;
        go = this.gameObject;
     

        //if (go.transform.childCount > 0)
        //{
        //    Transform[] ch = go.GetComponentsInChildren<Transform>();

        //    foreach (Transform c in ch)
        //    {

        //        DestroyImmediate(c.gameObject);

        //    }


        //}

        spriteCollection = go.transform.parent.GetComponent<tk2dSprite>().Collection;
        selectedSpriteName = go.transform.parent.GetComponent<tk2dSprite>().CurrentSprite.name;

        tk2dStaticSpriteBatcher batcher;

        //if (!this.gameObject.GetComponent<tk2dStaticSpriteBatcher>())
        //{
        //     batcher = this.gameObject.AddComponent<tk2dStaticSpriteBatcher>();
        //}
        //else
        //{

             batcher = this.gameObject.GetComponent<tk2dStaticSpriteBatcher>();
        //}

        batcher.batchedSprites = new tk2dBatchedSprite[points.Length];

        GameObject o = GameObject.CreatePrimitive(PrimitiveType.Quad);

      
        for (int i = 0; i < batcher.batchedSprites.Length; ++i)
        {

          
            tk2dBatchedSprite bs = new tk2dBatchedSprite();

            // assign sprite collection and sprite Id for this batched sprite
            bs.spriteCollection = spriteCollection;
            bs.spriteId = spriteCollection.GetSpriteIdByName(selectedSpriteName);


            


            Vector3 pos = points[i].position;

            o.transform.position = pos;

            o.transform.LookAt(gameObject.transform);


            patternIndex2++;
            Debug.Log("poot");

            if (patternIndex2 == patternbools.Length)
            {
                patternIndex2 = 0;
            }

            if (patternbools[patternIndex2] == false)
            {
                 pos = new Vector3(0f, 0f, 0f);
            }



            // Just lookat
            o.transform.LookAt(gameObject.transform);

            Quaternion q = o.transform.rotation;

            Quaternion qr = Quaternion.Euler(new Vector3(0f, 0f, o.transform.rotation.eulerAngles.z));

           // Debug.Log(points[i].position);

            bs.relativeMatrix.SetTRS(pos, q , Vector3.one * spriteScale);
            
            //Quaternion.identity

           // bs.position = pos;

            //bs.rotation = qr;
            
            //bs.localScale = Vector3.one * spriteScale;

            batcher.batchedSprites[i] = bs;

            //bs.rotation = qr;

             // batcher.batchedSprites[i].localScale =  Vector3.one * spriteScale;

            i = i + stepSize;
        }

        // Don't create colliders when you don't need them. It is very expensive to
        // generate colliders at runtime.
        batcher.SetFlag(tk2dStaticSpriteBatcher.Flags.GenerateCollider, false);

        batcher.Build();

        boolupdate = false;
       // return;
    }

    void Awake()
    {
        instance = this;
        theta = 0;
        resolution = numArms * numPPA;
        CreatePoints();
    }
    public static Grapher2 getInstance()
    {
        return instance;
    }
}