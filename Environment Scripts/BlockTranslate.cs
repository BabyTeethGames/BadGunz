using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTranslate : MonoBehaviour {

    List<CubePlatform> blocks;
    public float movementFrequency = 0.5f;
    public float blockSpeed = 1.5f;
    private float nextMove = 0.2f;
    private float blockTime;
    private int choice;

    void Start()
    {
        blocks = new List<CubePlatform>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("MovingPlatform"))
            {
                blocks.Add(child.GetComponent<CubePlatform>());
            }
        }
    }

    void Update()
    {
        blockTime += Time.deltaTime;
        if (blockTime > nextMove)
        {
            choice = Random.Range(0, blocks.Count);
            if(!blocks[choice].moving)
            {
                blocks[choice].GetMove(RandomTarget(blocks[choice].transform), blockSpeed);
            }
            nextMove = blockTime + movementFrequency;
        }
    }


    private Vector3 RandomTarget(Transform block)
    {
        float y = Random.Range(0f, 15f);
        Vector3 target = new Vector3(block.position.x, y, block.position.z);
        return target;
    }
    
    
    /*
    IEnumerator Move(Transform block, int selection)
    {
        Vector3 prev = block.position;
        Vector3 veloc = Vector3.zero;

        yield return null;
        float y = Random.Range(0f, 15f);
        Vector3 target = new Vector3(block.position.x, y, block.position.z);
        //target = transform.InverseTransformPoint(target);
      
        while(Vector3.Distance(block.transform.position, target) > 0.05f)
        {     
            block.position = Vector3.MoveTowards(block.position, target, blockSpeed * Time.deltaTime);
            veloc = (block.position - prev) / Time.deltaTime;
            prev = block.position;
            yield return new WaitForFixedUpdate();
        }
        moving.Remove(selection);
        yield return null;
    }
*/
}
