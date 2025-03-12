using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HurtAnim()
    {
        animator.SetTrigger("Hurt");
    }
    public void WinAnim()
    {
        animator.SetTrigger("Win");
    }
    public void DieAnim()
    {
        animator.SetTrigger("Die");
        StartCoroutine(DeactivateAnimator());
    }
    IEnumerator DeactivateAnimator()
    {

        yield return new WaitForSecondsRealtime(1);
        gameObject.SetActive(false);
    }

}
