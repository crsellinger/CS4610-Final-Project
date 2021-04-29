using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float rotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    public Transform obstruction;
    float zoomSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        camControl();
        isObstructed();
    }

    void camControl(){
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);  //restrains camera from flipping around player

        transform.LookAt(Target);

        if (Input.GetKey(KeyCode.LeftAlt)) {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);  //if left alt is held down it only rotates camera
        }
        else {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);  //camera rotate
            Player.rotation =  Quaternion.Euler(0, mouseX, 0);      //player rotate
        }
    }

    //NOTE: when adding new game objects, make sure to add tags for walls and ground
    void isObstructed(){
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 4.5f)) {
            if (hit.collider.gameObject.tag != "Player") {   //if gameObject is not player
                obstruction = hit.transform;
                //keeps shadows from object in game while getting rid of obstructing object
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                //checks distance between object and player to decide to zoom to object or player
                if (Vector3.Distance(obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f) {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
            }
            else {  //else no obstruction turn object back on
                if (gameObject.GetComponent<MeshRenderer>() == null) {
                    return;
                }
                else {
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

                    if (Vector3.Distance(transform.position, Target.position) < 4.5f) {
                        transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }
}
