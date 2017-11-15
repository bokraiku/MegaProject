using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour {

    public string adv_1 = "Advanture1";
    public string world_1 = "World1";
    public string lobby = "LobbyScence";
    private Animator inv_anim;
    public GameObject inv;

    public GameObject MainCanvas;
    public GameObject UICanvas;

    public GameObject Player;
    public GameObject PlayerCamera;

    private NetworkManager networkManager;


    public Camera m_camera;
    private ToolTip tooltip;
    public Inventory inv_obj;

    public Image loadingBar;
    public GameObject LoadingPanel;

    AsyncOperation ao;

    private void Start()
    {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
       if (GameObject.FindWithTag("CharacterCanvas") != null)
        {
            //inv = GameObject.FindWithTag("CharacterCanvas").transform.Find("InventoryPanel").gameObject;
            inv_anim = inv.GetComponent<Animator>();
            Debug.Log("Inventory ANIM: " + inv_anim);
            //inv_obj = GameObject.Find("Inventory").GetComponent<Inventory>();
            tooltip = inv_obj.GetComponent<ToolTip>();

            
        }

        
        loadingBar.gameObject.SetActive(false);
        loadingBar.fillAmount = 0f;

        m_camera = Camera.main;

    }



    public void BT_Advanture()
    {
        //SceneManager.LoadScene(adv_1);
        if(!LoadingPanel.activeSelf )
        {
            LoadingPanel.SetActive(true);
        }
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadLevelWithRealProcess(world_1, lobby));
    }

    public void BT_World1()
    {
        //SceneManager.LoadScene(adv_1);
        if (!LoadingPanel.activeSelf)
        {
            LoadingPanel.SetActive(true);
        }
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadLevelWithRealProcess(adv_1, lobby));
    }

    public void BT_CloseInventory()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerCamera = Player.transform.Find("PlayerCamera").gameObject;

        tooltip.Deactivate();
        //UICanvas.SetActive(false);
        inv_anim.SetBool("IsOpen", false);
        MainCanvas.SetActive(true);
        if (PlayerCamera.activeSelf)
        {
            PlayerCamera.SetActive(false);
            Player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;
        }
        if (!m_camera.isActiveAndEnabled)
        {
            m_camera.enabled = true;
        }
    }
    public void BT_OpenInventory()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerCamera = Player.transform.Find("PlayerCamera").gameObject;

        if (!PlayerCamera.activeSelf)
        {
            PlayerCamera.SetActive(true);
            Player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = false;       
        }
        if (m_camera.isActiveAndEnabled)
        {
            m_camera.enabled = false;
        }
       
        //if (!m_camera.enabled )
        //PlayerCamera.SetActive(true);
        MainCanvas.SetActive(false);
        UICanvas.SetActive(true);
        inv.SetActive(true);
        inv_anim.SetBool("IsOpen", true);


    }

    public IEnumerator LoadLevelWithRealProcess(string scenseName,string old)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.UnloadSceneAsync(old);
        ao = SceneManager.LoadSceneAsync(scenseName);
        //networkManager.ServerChangeScene(scenseName);
        
        ao.allowSceneActivation = false;

        
        while (!networkManager.clientLoadedScene)
        {


            if (ao.progress == 0.9f)
            {
                loadingBar.fillAmount = 1f;
                ao.allowSceneActivation = true;
            }

            //loadtime += Time.deltaTime;

            loadingBar.fillAmount = ao.progress;
            //if (networkManager.clientLoadedScene)
            //{
            //    loadingBar.fillAmount = 1f;
            //}
            //Debug.Log(ao.progress);
            yield return null;
        }

       
        





    }
}
