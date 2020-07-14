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
		
}
