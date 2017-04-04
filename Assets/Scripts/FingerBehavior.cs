using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerBehavior : MonoBehaviour {

	LineRenderer lineRenderer;
	public AnimationCurve fingerAnimationCurve;
	EdgeCollider2D collider;
	public Vector3 targetPosition;
	Rigidbody2D tip;
	public float fingerResolution;
	public float moveTime;
	bool initialized;
	// Use this for initialization
	void Start () {
		initialized = false;
		lineRenderer = GetComponent<LineRenderer>();
		fingerAnimationCurve = new AnimationCurve();
		collider = GetComponent<EdgeCollider2D>();
		//InitializeFinger(new Vector3(0.5f, -3));
	}
	
	// Update is called once per frame
	void Update () {
		if(initialized) {
			UpdateKeyFrames();
			UpdateFingerPoints();
		}
	}

	public void MoveToTarget(Vector3 target) {
		if(initialized) {
			targetPosition = target;
			StartCoroutine(AnimateFingerMovement());
		}
		else {
			InitializeFinger(target);
		}
	}

	public void InitializeFinger(Vector3 target) {
		if(lineRenderer.numPositions > 1) {
			return;
		}
		targetPosition = target;
		float angleRad = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x);
		transform.rotation = Quaternion.Euler(0, 0, angleRad * Mathf.Rad2Deg);

		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).localPosition = new Vector3(transform.GetChild(i).localPosition.x, Random.value);
		}
		transform.GetChild(transform.childCount - 1).position = transform.worldToLocalMatrix.MultiplyPoint(target);

		fingerAnimationCurve.AddKey(new Keyframe(0, 0));
		Vector2[] colliderPoints = new Vector2[transform.childCount];
		for(int i = 1; i < transform.childCount; i++) {
			fingerAnimationCurve.AddKey(new Keyframe(transform.GetChild(i - 1).localPosition.x , transform.GetChild(i - 1).localPosition.y));
			colliderPoints[i - 1] = new Vector2(transform.GetChild(i - 1).localPosition.x, transform.GetChild(i - 1).localPosition.y);
		}
		collider.points = colliderPoints;
		StartCoroutine(AnimateFingerInit());
		tip = transform.GetChild(transform.childCount - 1).GetComponent<Rigidbody2D>();
	}

	void UpdateFinger() {
		tip.MovePosition(targetPosition);
	}

	void UpdateKeyFrames() {
		fingerAnimationCurve.keys = new Keyframe[0];
		fingerAnimationCurve.AddKey(new Keyframe(0, 0));
		Vector2[] colliderPoints = new Vector2[transform.childCount];
		for(int i = 1; i < transform.childCount + 1; i++) {
			fingerAnimationCurve.AddKey(new Keyframe(transform.GetChild(i - 1).localPosition.x, transform.GetChild(i - 1).localPosition.y));
			colliderPoints[i - 1] = new Vector2(transform.GetChild(i - 1).localPosition.x, transform.GetChild(i - 1).localPosition.y);
		}
		collider.points = colliderPoints;
	}

	void UpdateFingerPoints() {
		
		float i = 0;
		//while(i < fingerDensity) {
		Vector3[] positions = new Vector3[lineRenderer.numPositions];
		lineRenderer.GetPositions(positions);
		float distancePerPoint = (float)fingerAnimationCurve.keys[fingerAnimationCurve.keys.Length - 1].time / (float)lineRenderer.numPositions;
		for(int k = 0; k < lineRenderer.numPositions; k++){	
			lineRenderer.SetPosition(k, new Vector3(distancePerPoint * k, fingerAnimationCurve.Evaluate(distancePerPoint * k)));
		}
	}

	IEnumerator AnimateFingerInit() {
		float t = 0;
		while(t < Vector3.Distance(Vector3.zero, transform.worldToLocalMatrix.MultiplyPoint(targetPosition))) {
			lineRenderer.numPositions = lineRenderer.numPositions + 1;
			lineRenderer.SetPosition(lineRenderer.numPositions -1, new Vector3(t, fingerAnimationCurve.Evaluate(t)));
			t += Time.deltaTime * 60;
			yield return null;
		}
		tip.MovePosition(targetPosition);
		initialized = true;
	}

	IEnumerator AnimateFingerMovement() {
		Vector3 currentPos = tip.transform.position;
		for(float t = 0; t < moveTime; t += Time.deltaTime) {
			tip.MovePosition(Vector3.Lerp(currentPos, targetPosition, t / moveTime));
			yield return null;
		}
		tip.MovePosition(targetPosition);
	}
}