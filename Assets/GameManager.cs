using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject[] disks;
    public GameObject[] diskSequence;
    public Vector3[] posASequence;
    public Vector3[] posBSequence;
    public Canvas uiCanvas;

    public int n = 6;
    public float AnimationSpeed = 1;


    private Vector3 _wayPointA;
    private Vector3 _wayPointB;
    private Vector3 _wayPointC;

    private int state = 0;
    private int i = -1;
    


    void Start()
    {
        _wayPointA = GameObject.Find("WaypointA").transform.position;
        _wayPointB = GameObject.Find("WaypointB").transform.position;
        _wayPointC = GameObject.Find("WaypointC").transform.position;

        diskSequence = new GameObject[150];
        posASequence = new Vector3[150];
        posBSequence = new Vector3[150];
    }

    public void StartGame()
    {
        Hanoi(n, _wayPointA, _wayPointC, _wayPointB);

        for (int b = 0; b < Mathf.Pow(2, n) - 1; b++)
        {
            Debug.Log($"Move disk {diskSequence[b]} from {posASequence[b]} to {posBSequence[b]}");
        }

        StartCoroutine(MoveAllDisks());

        uiCanvas.enabled = false;
    }

    IEnumerator MoveAllDisks()
    {
        for (int a = 0; a < Mathf.Pow(2, n) - 1; a++)
        {
            yield return StartCoroutine(Move(diskSequence[a], posASequence[a],posBSequence[a]));
            if (a == Mathf.Pow(2, n) - 3)
                state++;
        }
    }

    void Hanoi(int n, Vector3 from, Vector3 to, Vector3 aux)
    {
        if (n == 1)
        {
            AddToSequence(disks[0], from, to);
            return;
        }
        
        Hanoi(n - 1, from, aux, to);
        AddToSequence(disks[n-1], from, to);
        Hanoi(n - 1, aux, to, from);
    }

    void AddToSequence(GameObject disk, Vector3 from, Vector3 to)
    {
        i++;
        diskSequence[i] = disk;
        posASequence[i].Set(from.x, from.y, from.z);
        posBSequence[i].Set(to.x,to.y,to.z);
    }

    IEnumerator Move(GameObject disk, Vector3 posA, Vector3 posB)
    {
        iTween.MoveTo(disk, posA, 2/AnimationSpeed);
        yield return new WaitForSeconds(2 / AnimationSpeed);
        iTween.MoveTo(disk, posB, 1 / AnimationSpeed);
        yield return new WaitForSeconds( 1 / AnimationSpeed);

        RaycastHit hit;

        if (Physics.Raycast(posB + Vector3.down * 2, Vector3.down, out hit, 6))
        {
            iTween.MoveTo(disk, hit.point + Vector3.up * .3f , 2 / AnimationSpeed);
            yield return new WaitForSeconds(2 / AnimationSpeed);
        }
    }

}
