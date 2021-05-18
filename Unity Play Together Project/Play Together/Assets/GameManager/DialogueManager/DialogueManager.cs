using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject requestCanvasPrefab;
    public GameObject waitingCanvasPrefab;
    GameObject waitingCanvas;
    public GameObject alertCanvasPrefab;
    public void displayWaitingCanvas(string onWhatScript)
    {
        Debug.Log("displayWaitingCanvas made by " + onWhatScript);
        waitingCanvas = Instantiate(waitingCanvasPrefab, waitingCanvasPrefab.transform.position, Quaternion.identity);
    }
    public void destroyWaitingCanvas()
    {
        Debug.Log("destroyWaitingCanvas");
        if (waitingCanvas)
        {
            Destroy(waitingCanvas);
        }
    }
    public void displayAlertCanvas(string title, string alert)
    {
        destroyWaitingCanvas();

        GameObject alertCanvas = Instantiate(alertCanvasPrefab, alertCanvasPrefab.transform.position, Quaternion.identity);

        TextMeshProUGUI titleTextGui = alertCanvas.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        titleTextGui.text = title;

        TextMeshProUGUI alertTextGui = alertCanvas.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();
        alertTextGui.text = alert;

        Button okButton = alertCanvas.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<Button>();
        okButton.onClick.AddListener(delegate () { Destroy(alertCanvas); });

        Button closeButton = alertCanvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate () { Destroy(alertCanvas); });
    }

    public void displayRequestCanvas(string title, string statement, Action<bool, object[]> callBackFunction, object[] staticObjects)
    {

        GameObject requestCanvas = Instantiate(requestCanvasPrefab, requestCanvasPrefab.transform.position, Quaternion.identity);

        Button acceptButton = requestCanvas.transform.GetChild(0).GetChild(1).GetChild(6).GetComponent<Button>();
        acceptButton.onClick.AddListener(delegate ()
        {
            callBackFunction(true, staticObjects);
            Destroy(requestCanvas);
        });

        Button rejectButton = requestCanvas.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<Button>();
        rejectButton.onClick.AddListener(delegate ()
        {
            callBackFunction(false, staticObjects);
            Destroy(requestCanvas);
        });

        Button closeButton = requestCanvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate () { Destroy(requestCanvas); });

        requestCanvas.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = title;

        requestCanvas.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = statement;
    }

}
