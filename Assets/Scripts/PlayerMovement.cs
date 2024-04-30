using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float rotSpeed = 180f;
    float shipBoundarRadius = 0.5f;

    bool canDash = true;
    bool isDashing;
    float dashingPower = 4f;
    float dashingTime = 0.15f;
    float dashingCooldown = 1f;

    bool isInvulnerable = false;
    public float actInvulnPeriod = 5;
    float invulnTimer = 0;
    int correctLayer;

    // TrailRenderer tr;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // ROTATE the ship

        // Grab our rotation quarterion
        Quaternion rot = transform.rotation;

        // Grab the z euler angle
        float z = rot.eulerAngles.z;

        // hange the Z angle based on input
        z -= Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;

        // Recreate th quarteniuon
        rot = Quaternion.Euler(0, 0, z);

        // Feed the quaternion into our rotation
        transform.rotation = rot;


        // MOVE the ship
        // Returns a float from -1.0 to 1.0
        // Input.GetAxis("Vertical");
        Vector3 pos = transform.position;

        Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime, 0);

        // pos.y += Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime;
        pos += rot * velocity;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("Invulnerable");
            StartInvulnerability();
        }

        if (isInvulnerable)
        {
            invulnTimer -= Time.deltaTime;
            if (invulnTimer <= 0)
            {
                EndInvulnerability();
            }
        }

        // Restric the player to the camera's boundaries

        // Vertical
        if (pos.y + shipBoundarRadius > Camera.main.orthographicSize)
        {
            pos.y = Camera.main.orthographicSize - shipBoundarRadius;
        }
        if (pos.y - shipBoundarRadius < -Camera.main.orthographicSize)
        {
            pos.y = -Camera.main.orthographicSize + shipBoundarRadius;
        }

        // Calculate the orthographic width based on the screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height; // WARNING, Will be weard when de whidth is smaller then the height
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + shipBoundarRadius > widthOrtho)
        {
            pos.x = widthOrtho - shipBoundarRadius;
        }
        if (pos.x - shipBoundarRadius < -widthOrtho)
        {
            pos.x = -widthOrtho + shipBoundarRadius;
        }

        transform.position = pos;

    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        // tr.emitting = true;

        // Store the original maxSpeed
        float originalMaxSpeed = maxSpeed;
        float originalMaxRotSpeed = rotSpeed;

        // Increase maxSpeed during dash
        maxSpeed *= dashingPower;
        rotSpeed *= dashingPower/2;

        // Wait for dashingTime
        yield return new WaitForSeconds(dashingTime);

        // Reset maxSpeed
        maxSpeed = originalMaxSpeed;
        rotSpeed = originalMaxRotSpeed;

        // tr.emitting = true;

        // Cooldown
        yield return new WaitForSeconds(dashingCooldown);


        // Allow dash again
        canDash = true;
        isDashing = false;
    }

    void StartInvulnerability()
    {
        isInvulnerable = true;
        gameObject.layer = 8;
        Debug.Log("gamelayer"); // Set to invulnerable layer
        invulnTimer = actInvulnPeriod; // Set the timer
    }

    void EndInvulnerability()
    {
        isInvulnerable = false;
        gameObject.layer = correctLayer;
        Debug.Log("gamelayer acabou"); // Restore correct layer
    }

    



}
