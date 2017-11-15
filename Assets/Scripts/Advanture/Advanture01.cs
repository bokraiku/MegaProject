using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Advanture01 : MonoBehaviour {

    // Use this for initialization
    public float StartNumber;
    public Text UIText;
    public GameObject StartPanel;
    private GameObject PlayerObject;

    public GameObject ResultPanel;

    public GameObject Wall1;
    public GameObject Wall2;
    public GameObject Wall3;

    public GameObject MonGroup1;
    public GameObject MonGroup2;
    public GameObject Boss;
    public UIManager uiManager;

    private bool state = false;


    private void Awake()
    {
        ResultPanel.SetActive(false);
    }

    void Start () {
        StartNumber = 5;
        UIText.text = ((int)StartNumber).ToString();
        //PlayerObject = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().gameObject;

        //PlayerObject.SetActive(false);

    }
    void Update()
    {
        StartNumber -= Time.deltaTime;
        UIText.text = Mathf.Floor(StartNumber).ToString();
        if (StartNumber <= 0)
        {
            UIText.text = "0";
            StartPanel.SetActive(false);
            //PlayerObject.SetActive(true);
            Wall1.SetActive(false);
        }

        // Check Mon1
        this.ManageGroup1();
        this.ManageGroup2();
        this.ManageBoss();

    }

    private void ManageGroup1()
    {
        Debug.Log("Count Mon1 : " + MonGroup1.transform.childCount);
        int countMon = MonGroup1.transform.childCount;
        if(countMon == 0 && this.Wall2.activeSelf == true)
        {
            this.Wall2.SetActive(false);
        }
    }

    private void ManageGroup2()
    {
        Debug.Log("Count Mon2 : " + MonGroup2.transform.childCount);
        int countMon = MonGroup2.transform.childCount;
        if (countMon == 0 && this.Wall3.activeSelf == true)
        {
            this.Wall3.SetActive(false);
        }
    }
    private void ManageBoss()
    {
        Debug.Log("Count Boss : " + Boss.transform.childCount);
        int countMon = Boss.transform.childCount;
        if (countMon == 0 && state == false)
        {
            state = true;
            //this.Wall3.SetActive(false);
            StartCoroutine(ShowResult());
        }
    }

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1);
        ResultPanel.SetActive(true);
        StartCoroutine(LoadNewState());
    }

    private IEnumerator LoadNewState()
    {
        yield return new WaitForSeconds(3f);
        ResultPanel.SetActive(false);
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (!uiManager.LoadingPanel.activeSelf)
        {
            uiManager.LoadingPanel.SetActive(true);
        }
        uiManager.loadingBar.gameObject.SetActive(true);
        StartCoroutine(uiManager.LoadLevelWithRealProcess(uiManager.lobby, uiManager.adv_1));

    }



}
