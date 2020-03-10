using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int rotatespeed;
    private GameObject pacman;
    private float startTime;
    public int i, j, ni, nj;
    public float y;
    private string estado = "wait";
    Animator floaty;
      

    public float speed = 3;
    public static readonly int[][] connected4 = new int[4][] { new int[2] { -1, 0 }, new int[2] { 1, 0 }, new int[2] { 0, -1 }, new int[2] { 0, 1 } };

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        /*i = 14;
        j = 14;
        ni = 15;
        nj = 14;*/
        gameObject.transform.position = new Vector3(-0.75f, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (pacman == null)
        {
            pacman = GameObject.FindWithTag("Player");
            return;
        }

        transform.Rotate(0, rotatespeed * Time.deltaTime, 0);

        Vector3 source = new Vector3((-GameManager.bound(0) / 2) + i, y, (-GameManager.bound(1) / 2) + j);
        Vector3 dest = new Vector3((-GameManager.bound(0) / 2) + ni, y, (-GameManager.bound(1) / 2) + nj);

        float distCovered = (Time.time - startTime) * speed;

        transform.position = Vector3.Lerp(source, dest, distCovered);

        if (distCovered >= 1.0f) // Já chegou no destino
        {
            startTime = Time.time; // reseta relógio
            int ti = 0, tj = 0;
            float distance = 100;
            Vector3 search = new Vector3((-GameManager.bound(0) / 2) + ni, y, (-GameManager.bound(1) / 2) + nj);

            foreach (int[] point in connected4)
            {
                if (((GameManager.map[ni + point[0], nj + point[1]] < 8) && !(i == ni + point[0] && j == nj + point[1])) | (estado == "sair" && (GameManager.map[ni + point[0], nj + point[1]] == 9)))
                {
                    if (estado == "chase")
                    {
                        float dist_tmp = Vector3.Distance(pacman.transform.position, search + new Vector3(point[0], 0, point[1]));
                        if (dist_tmp < distance)
                        {
                            distance = dist_tmp;
                            ti = point[0];
                            tj = point[1];
                        }
                    }
                    else if (estado == "wait")
                    {
                        if (point[0] == 1 | point[0] == -1)
                        {
                            ti = point[0];
                            tj = point[1];
                        }
                    }
                    else if (estado == "sair")
                    {

                        float dist_tmp = Vector3.Distance(new Vector3(-4, 1, 0.5f), search + new Vector3(point[0], 0, point[1]));
                        if (dist_tmp < distance)
                        {
                            distance = dist_tmp;
                            ti = point[0];
                            tj = point[1];
                        }
                        if ((GameManager.map[i + point[0], j + point[1]] == 9))
                        {
                            estado = "chase";
                        }
                    }

                }
            }
            i = ni;
            j = nj;
            ni += ti;
            nj += tj;
        }
    }

    public void setEstado(string setState)
    {
        estado = setState;
    }
}

