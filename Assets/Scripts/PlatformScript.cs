using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
//Script brought to you by the fucking goat
public class PlatformScript : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Collider2D platformCollider;
    private Collider2D playerCollider;
    private bool isDropping;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        platformCollider = GetComponent<Collider2D>();
        if (playerMovement != null)
        {
            playerCollider = playerMovement.GetComponent<Collider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ElMetodo();
    }
    private IEnumerator CollisonReturned(){
        isDropping = true;
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
        isDropping = false;
    }
    private void ElMetodo(){ //muy caliente
        if(playerMovement == null || platformCollider == null || playerCollider == null || isDropping){
            return;
        }

        if(playerMovement.moveInput.y < 0 && playerMovement.Grounded() && platformCollider.IsTouching(playerCollider)){
            StartCoroutine(CollisonReturned());
        }
    }
}
