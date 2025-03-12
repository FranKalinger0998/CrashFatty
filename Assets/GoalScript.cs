using UnityEditor.Build.Content;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public PlayerScript playerAcross;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {   
            if(other.TryGetComponent<ColoredBall>(out ColoredBall script))
            {
                script.ReturnToPool();
                if (script.currentColor == playerScript.selectedColor)
                {
                    GameManager.Instance.RemovePoints(playerScript.selectedColor, 3);
                }
                else if (script.currentColor == playerAcross.selectedColor)
                {
                    GameManager.Instance.RemovePoints(playerScript.selectedColor, 1);
                }
                else
                {
                    GameManager.Instance.RemovePoints(playerScript.selectedColor, 2);
                }
               
            }
            else
            {
                if(other.TryGetComponent<GoldenBall>(out GoldenBall scriptx))
                {
                    scriptx.ReturnToPool();
                }
                //scriptg.ReturnToPool();
                GameManager.Instance.RemovePoints(playerScript.selectedColor, 5);
                
            }
            //GameManager.Instance.RemoveBallToPlay(other.transform);
            //BotController[] c = FindObjectsByType<BotController>(FindObjectsSortMode.InstanceID);
            //foreach (BotController item in c)
            //{
            //    item.UpdateBallList();
            //}

        }
    }
}
