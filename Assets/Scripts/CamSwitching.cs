using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamSwitching : MonoBehaviour {

    public CinemachineVirtualCamera mainCam;
    public GameObject highlightPrefab;

    private GameObject bestCar;
    private SceneTextScript EvoManager;
    private GameObject instantiatedHighlight;
    private CinemachineVirtualCamera activeCam;

    void Start() {
        activeCam = mainCam;
        EvoManager = GameObject.FindGameObjectWithTag("EvoManager").GetComponent<SceneTextScript>();
    }

    void Update() {
        bestCar = GetBestCar();
    }

    //P�ep�n�n� mezi bestCar a main kamerou (pro tla��tko v UI)
    public void SwitchMainCam() {
        if (mainCam.enabled) {
            ActivateBestCarCam();
        } else {
            ActivateMainCam();
        }
    }

    void ActivateMainCam() {
        mainCam.enabled = true;
    }
    void ActivateBestCarCam() {
        SwitchCam(GetVcamChild(bestCar));
    }
    GameObject GetBestCar() {
        return EvoManager.BestCar;
    }
    //Nejle�p� auto sv�t� fialov�
    void HighlightBestCar() {
        Destroy(instantiatedHighlight);
        instantiatedHighlight = Instantiate(highlightPrefab, bestCar.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        instantiatedHighlight.transform.parent = bestCar.transform;
    }
    //P�ep�n�n� na zvolenou kamerou
    void SwitchCam(CinemachineVirtualCamera camToActivate) {
        activeCam.enabled = false;
        camToActivate.enabled = true;
    }
    //Najde child object co obsahuje vcam
    CinemachineVirtualCamera GetVcamChild(GameObject origin) {
        CinemachineVirtualCamera vcam = origin.GetComponentInChildren<CinemachineVirtualCamera>(true);
        if (vcam != null) {
            return vcam;
        }
        return null;
    }
}
