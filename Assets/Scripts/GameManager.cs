using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    private List<GameObject> arrivees = new List<GameObject>();
    private List<GameObject> restants = new List<GameObject>();

    public List<GameObject> gates = new List<GameObject>();
    public List<GameObject> finishLines = new List<GameObject>();


    public TextMeshProUGUI eliminationText;
    public TextMeshProUGUI winText;

    public Camera mainCamera;

    private int gatesIndex = 0;
    private int finishLinesIndex = 0;

    public GameObject gatePrefab;

    private float currentSpeed = 1f;



    void Start()
    {
        // Assurez-vous que le messageText est désactivé au début
        if (eliminationText != null)
        {
            eliminationText.gameObject.SetActive(false);
        }
        if(winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        restants.AddRange(GameObject.FindGameObjectsWithTag("Marble").ToList());
        Debug.Log(restants.Count);
        StartCoroutine(Wait(2f));

    }

    public void SetGameSpeed(float speed)
    {
        currentSpeed = speed;

        Time.timeScale = currentSpeed;
        Time.fixedDeltaTime = 0.02f / currentSpeed;

        Debug.Log("Vitesse du jeu réglée sur x" + currentSpeed);
    }

    public void BilleArrivee(GameObject bille)
    {
        if (!arrivees.Contains(bille))
        {
            arrivees.Add(bille);
            restants.Remove(bille);
            Debug.Log(bille.name + " est arrivée");

            if (restants.Count == 1)
            {
                EliminerDerniere();
                StartCoroutine(WaitAndStartNextRound());
            }
        }

    }

    private IEnumerator WaitAndStartNextRound()
    {
        yield return new WaitForSeconds(1f); // attendre 1 secondes
        StartNextRound();
    }

    void EliminerDerniere()
    {
        if (arrivees.Count >= 1)
        {
            int position = arrivees.Count + 1;
            GameObject derniereBille = restants[0];
            Debug.Log("Éliminé : " + derniereBille.name);
            arrivees.Remove(derniereBille);
            Destroy(derniereBille);

            Destroy(finishLines[finishLinesIndex]);
            finishLinesIndex++;

            string pos = "";
            switch (position % 10)
            {
                case 1:
                    pos = "st ";
                    break;
                case 2:
                    pos = "nd ";
                    break;
                case 3:
                    pos = "rd ";
                    break;
                default:
                    pos = "th ";
                    break;
            }
            if (position >= 11 && position <= 13)
            {
                pos = "th ";
            }

            Time.timeScale = 0.2f;
            eliminationText.text = position + pos + derniereBille.name + " was eliminated !";
            eliminationText.gameObject.SetActive(true);

            // Réinitialiser la liste pour le prochain round
            arrivees.Clear();
        }
     
    }
    void StartNextRound()
    {
        // Réinitialiser le jeu pour le prochain round
        eliminationText.gameObject.SetActive(false);
        mainCamera.transform.position = mainCamera.transform.position + new Vector3(0, -24, 0);
        Time.timeScale = 1f; // Réinitialiser le temps

        StartCoroutine(WaitBeforeDestroy(1f));
        restants.Clear();
        restants.AddRange(GameObject.FindGameObjectsWithTag("Marble").ToList());

        if(restants.Count == 1)
        {
            GameObject gagnante = restants[0];
            Debug.Log(gagnante.name + " a gagné !");
            winText.text = gagnante.name + " a gagné !";
            winText.gameObject.SetActive(true);
            return;
        }
        Debug.Log("Prochain round commencé");

    }
    private IEnumerator WaitBeforeDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gates[gatesIndex]);
        gatesIndex++;
    }

    private IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gatePrefab);
        Debug.Log("Gate destroyed after delay");
    }
}

