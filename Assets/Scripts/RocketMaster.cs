using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketMaster : MonoBehaviour
{
    public TextMeshProUGUI speed, height, maxHeight;
    public TMP_InputField heightInput;

    public GameObject warningMessage;
    public Rigidbody rigidBody;
    public CapsuleCollider capsuleCol;
    public BoxCollider boxCol;
    public float force;
    public bool fiveSecondsOfFuel;

    public GameObject firstStage, secondStage, parachute, thrustEmission, terrainHolder;
    public ParticleSystem thrustParticle;
    public AudioSource thrustSound;

    public Vector3 windDirectionInVector3;
    public float windSpeed = 3;

    bool launch;
    float targetHeight, heightVal, maxHeightValue;

    bool stage2, decoupled, reachedTarget, timesUP, thrusting;

    Vector3 moveDirection;
    Vector3 lastPos = Vector3.zero;

    private void Start()
    {
        thrustEmission.transform.position = firstStage.transform.position;
    }

    private void Update()
    {
        if (thrusting && !thrustParticle.isPlaying)
        {
            thrustParticle.Play();
            var emission = thrustParticle.emission;
            emission.enabled = true;
            thrustSound.Play();
        }
        else if (!thrusting && !thrustParticle.isStopped)
        {
            thrustParticle.Stop();
            var emission = thrustParticle.emission;
            emission.enabled = false;
            thrustSound.Stop();
        }

        if (transform.position.y > 400)
        {
            terrainHolder.transform.position = new Vector3(terrainHolder.transform.position.x, terrainHolder.transform.position.y, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (!launch) return;
        rigidBody.AddForce(windDirectionInVector3 * windSpeed);

        speed.text = "Speed: " + Mathf.Round(rigidBody.velocity.magnitude * 100f) / 100f;
        height.text = "Height: " + Mathf.Round(transform.position.y * 100f) / 100f;
        heightVal = Mathf.Round(transform.position.y * 100f) / 100f;
        if (heightVal > maxHeightValue)
        {
            maxHeightValue = Mathf.Round(transform.position.y * 100f) / 100f;
            maxHeight.text = "Max Height: " + Mathf.Round(transform.position.y * 100f) / 100f;
        }

        moveDirection = transform.forward;
        if (fiveSecondsOfFuel && !timesUP)
        {
            Thrust();
            return;
        }

        if (reachedTarget) return;
        transform.forward = Vector3.Lerp(transform.forward, rigidBody.velocity.normalized, .01f);

        float h = rigidBody.velocity.y * rigidBody.mass;
        if (h + transform.position.y*2 < targetHeight / 2)
        {
            Thrust();
        }
        else if (!stage2)
        {
            thrusting = false;
            Decouple();
            stage2 = true;
        }

        if (!decoupled) return;

        if (h + transform.position.y*2 < targetHeight)
        {
            Thrust();
        }
        else if (h + transform.position.y*2 >= targetHeight)
        {
            thrusting = false;
            
            if (transform.position.y < lastPos.y)
            {
                DeployShute();
                reachedTarget = true;
            }
            lastPos = transform.position;
        }
    }

    void DeployShute()
    {
        parachute.SetActive(true);
    }

    public void Launch()
    {
        if (fiveSecondsOfFuel)
        {
            rigidBody.isKinematic = false;
            StartCoroutine(FiveSecondFlight());
            return;
        }

        targetHeight = float.Parse(heightInput.text);

        if (targetHeight < 100)
        {
            warningMessage.SetActive(true);
            return;
        }
        else
        {
            warningMessage.SetActive(false);
        }

        launch = true;
        rigidBody.isKinematic = false;
    }

    public void ToggleFiveSeconds()
    {
        fiveSecondsOfFuel = !fiveSecondsOfFuel;
        warningMessage.SetActive(false);
    }

    void Decouple()
    {
        if (firstStage.transform.parent == null) return;
        thrustEmission.transform.position = secondStage.transform.position;

        capsuleCol.center = new Vector3(0, 0, 0.84f);
        capsuleCol.height = 1.467324f;
        boxCol.enabled = false;

        firstStage.transform.parent = null;
        Rigidbody x = firstStage.AddComponent(typeof(Rigidbody)) as Rigidbody;
        CapsuleCollider y = firstStage.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
        y.center = new Vector3(0, 0, 0.525f);
        y.radius = 0.19f;
        y.height = 0.9656799f;
        x.AddForce(-moveDirection);

        Invoke("FinishDecouple", 1);
    }

    void FinishDecouple()
    {
        decoupled = transform;
        timesUP = false;
    }

    void Thrust()
    {
        rigidBody.AddForce(moveDirection * force);
        thrusting = true;
    }

    IEnumerator FiveSecondFlight()
    {
        launch = true;
        yield return new WaitForSeconds(2.5f);
        thrusting = false;
        timesUP = true;
        Decouple();
        yield return new WaitForSeconds(1.5f);
        thrusting = false;
        timesUP = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (decoupled)
        {
            parachute.SetActive(false);
            Invoke("Simplify", 10);
        }
    }

    void Simplify()
    {
        secondStage.transform.parent = null;
        Destroy(gameObject);
    }
}