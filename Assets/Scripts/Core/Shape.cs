using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {

	// turn this property off if you don't want the shape to rotate (Shape O)
	public bool m_canRotate = true;
    /*private void Start()
    {
        InvokeRepeating("moveDown", 0, .5f);
        InvokeRepeating("rotateRight", 0, .5f);
    }*/
    public Vector3 m_queueOffset;//fixing size of queue

   // public Vector3 m_holdOffset;//fixing size of hold shapes

    // general move method
    void move(Vector3 moveDirection)
	{
		transform.position += moveDirection;
	}


	//public methods for moving left, right, up and down, respectively
	public void moveLeft()
	{
		move(new Vector3(-1, 0, 0));
	}

	public void moveRight()
	{
		move(new Vector3(1, 0, 0));
	}

	public void moveUp()
	{
		move(new Vector3(0, 1, 0));
	}

	public void moveDown()
	{
		move(new Vector3(0, -1, 0));
	}


	//public methods for rotating right and left
	public void rotateRight()
	{
		if (m_canRotate)
			transform.Rotate(0, 0, -90);
	}
	public void rotateLeft()
	{
		if (m_canRotate)
			transform.Rotate(0, 0, 90);
	}
	
    public void rotateClockwise(bool ck)
    {
        if (ck)
        {
            rotateRight();
        }
        else
        {
            rotateLeft();
        }
    }

    //Effects
    GameObject[] m_glowSquareFX;
    public string gST = "LandShapeFX";

    private void Start()
    {
        if (gST != "")
        {
            m_glowSquareFX = GameObject.FindGameObjectsWithTag(gST);
        }
    }

    public void landShapeFX()
    {
        int i = 0;
        foreach(Transform child in gameObject.transform)
        {
            if (m_glowSquareFX[i])
            {
                m_glowSquareFX[i].transform.position = new Vector3(child.position.x, child.position.y, -2f);
                ParticlePlayer particlePlayer = m_glowSquareFX[i].GetComponent<ParticlePlayer>();

                if (particlePlayer)
                {
                    particlePlayer.play();
                }
            }
            i++;
        }
    }
}
