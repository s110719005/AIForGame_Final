using UnityEngine;

public class SniperPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject sniperPlayer;
    [SerializeField]
    private Camera sniperCamera;
    [SerializeField]
    private float horizontalSpeed = 5;
    [SerializeField]
    private float cameraSensitivity = 5;
    private float xRotation = 0f;
    LayerMask layerMask;
    private GameObject currentSelect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMask = LayerMask.NameToLayer("Character");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            sniperPlayer.transform.position -= sniperCamera.transform.right * horizontalSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            sniperPlayer.transform.position += sniperCamera.transform.right * horizontalSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            sniperPlayer.transform.position -= sniperCamera.transform.forward * horizontalSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.W))
        {
            sniperPlayer.transform.position += sniperCamera.transform.forward * horizontalSpeed * Time.deltaTime;
        }

        //mouse input
        {
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;

            sniperCamera.transform.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            sniperCamera.transform.localEulerAngles = new Vector3(xRotation, sniperCamera.transform.localEulerAngles.y, 0f);
        }

        // Ray ray = sniperCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        // Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);

        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        // {
        //     Debug.Log("RAYCAST HIT: " + hit.collider.name);
        // }

        RaycastHit hit;
        Debug.DrawRay(sniperCamera.transform.position, sniperCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.red); 
        if (Physics.Raycast(sniperCamera.transform.position, sniperCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        { 
            if(hit.collider.tag == "Character")
            {
                Debug.Log("RAYCAST HIT: " + hit.collider.name);
                if(hit.collider.gameObject.TryGetComponent<Outline>(out Outline outline))
                {
                    outline.OutlineWidth = 5;
                    currentSelect = hit.collider.gameObject;
                }
            }
            else if(currentSelect != null)
            {
                if(currentSelect.TryGetComponent<Outline>(out Outline outline))
                {
                    outline.OutlineWidth = 0;
                    currentSelect = null;
                }
            }
            //Debug.Log("RAYCAST CHATACTER"); 
        }
        else
        { 
            //Debug.DrawRay(transform.position + new Vector3(0, 3, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            //Debug.Log("Did not Hit"); 
        }

        if (Input.GetMouseButton(0) && currentSelect != null)
        {
            if(currentSelect.TryGetComponent<NPCMovement>(out NPCMovement npcMovement))
            {
                npcMovement.Kill();
            }
        }
    }
}
