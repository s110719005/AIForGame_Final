using System.Collections;
using UnityEngine;

public class SpyPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spyPlayer;
    [SerializeField] private float horizontalSpeed = 1;
    [SerializeField] private int rotationSpeed = 5;

    private Coroutine toIdleCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            //sniperPlayer.transform.Rotate(Vector3.up * Time.deltaTime);
            spyPlayer.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            spyPlayer.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            //spyPlayer.transform.position -= spyPlayer.transform.forward * horizontalSpeed * Time.deltaTime;
            animator.SetFloat("Vert", 1);
        }
        else
        {
            if(animator.GetFloat("Vert") > 0)
            {
                if(toIdleCoroutine != null)
                {
                    StopCoroutine(toIdleCoroutine);
                    toIdleCoroutine = null;
                }
                StartCoroutine(ChangeToIdleCoroutine());
            }
        }
    }

    private IEnumerator ChangeToIdleCoroutine()
    {
        float currentSpeed = 1;
        for(int i = 0; i < 50; i++)
        {
            currentSpeed -= 0.1f;
            animator.SetFloat("Vert", currentSpeed);
            if(currentSpeed <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        animator.SetFloat("Vert", 0);
        yield return null;
    }
}
