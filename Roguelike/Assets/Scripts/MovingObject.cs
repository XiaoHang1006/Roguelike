using UnityEngine;
using System.Collections;

public abstract class  MovingObject : MonoBehaviour {
	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverserMoveTime;

	protected virtual void Start()
	{
		rb2D = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		inverserMoveTime = 1 / moveTime;
	}

	protected  bool Move(int xDir,int yDir,out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end,blockingLayer);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}
		return false;	
	}

	public IEnumerator SmoothMovement(Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon) {
			Vector2 newPostion = Vector2.MoveTowards (transform.position, end, inverserMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPostion);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttempMove<T>(int xDir,int yDir) where T:Component
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null) {
			return;
		}

		T hitComponent = hit.transform.GetComponent<T> ();
		if (!canMove && hitComponent != null) {
			OnCanMove<T> (hitComponent);	
		}
	}

	protected abstract void OnCanMove<T> (T component) where T : Component;
}
