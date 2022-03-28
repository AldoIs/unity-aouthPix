using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Timers;
using UnityEngine.UI;

public class discordPixiConector : MonoBehaviour
{
    public bool consultar = false;
    public string urlLogin;
    public string KEY_API;
    public string prefijoClave;
    public string urlResponse;
    public Button button; // Drag & Drop the button in the inspector
    public Button checkLogin;
    public DatosDisc login; 
    private  Timer aTimer;
    private  int i;
    private GameObject respawn;
    public string response;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenURLLogin()
    {
       
        i = 0;
        string code = prefijoClave + ((System.DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
        string login = urlLogin + "?code_sesion=" + code + "&k=" + KEY_API; 
         response = urlResponse + "?code_session=" + code + "&k=" + KEY_API;
        Debug.Log("ULR PARA CONSULTAR: " + response);
        Debug.Log(":\nPrefijos: " + code + ":\n " + urlResponse);
        Application.OpenURL(login);
        TurnRed();
        //  SetTimer(); // Contador para poder hacer consultas
        // StartCoroutine(GetRequest(response));
        button.gameObject.SetActive(false);
        checkLogin.gameObject.SetActive(true);
    }

    public void getRes()
    {
        GetRequest(response);
    }

    public  void getRequestManual()
    {
       
        StartCoroutine(GetRequest(response));

    }


    IEnumerator GetRequest(string uri)
    {
       
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        try
                        {
                            consultar = false;

                            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                            login = DatosDisc.CreateFromJSON(webRequest.downloadHandler.text);

                            if (System.String.IsNullOrEmpty(login.idDiscord))
                            {
                                consultar = true;
                            }
                            
                            Debug.Log(":\nDatos: " + login.Apodo + ":\nDatos: ");

                        }
                        catch (System.Exception e)
                        {
                            consultar = true;
                            Debug.LogError(":\n Error en la informacion: " + e );
                        }
                        //SUCESS
                        if (!webRequest.downloadHandler.text.ToString().Contains("error"))
                        {
                            sucessLogin();
                            Debug.Log(webRequest.downloadHandler.text);
                        }
                        else
                        {
                            consultar = true;
                        }

                    }
                    
                   


                    break;
            }
        }
    }

    private  void SetTimer()
    {

        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(20000);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += new ElapsedEventHandler(timerLoop);
        aTimer.AutoReset = true;
        aTimer.Enabled = true;

        

    }

    private void timerLoop(object sender, System.EventArgs e)
    {
        if (consultar)
        {
            i++;
            Debug.Log("Intento Numero " + i);
        } else
        {
            aTimer.Enabled = false;
            Debug.Log("\nSe salio un total de:  " + i);
         
        }
       
    }

    private void sucessLogin()
    {
        GameObject.Find("usuario").GetComponent<Text>().text = "Bienvenido " + login.usernameDiscord + "#" + login.discriminator;
        respawn = GameObject.FindGameObjectsWithTag("Player")[0];
        respawn.GetComponent<PlayerController>().fuerza = (int)login.Fuerza;
        TurnWhite();
    }

    public void TurnRed()
     {
         ColorBlock colors = button.colors;
         colors.normalColor = Color.red;
         colors.highlightedColor = new Color32(255, 100, 100, 255);
         button.colors = colors;
        Debug.Log("Hola");
     }
     
     public void TurnWhite()
     {
         ColorBlock colors = button.colors;
         colors.normalColor = Color.white;
         colors.highlightedColor = new Color32(225, 225, 225, 255);
         button.colors = colors;
     }
}




[System.Serializable]
public class DatosDisc
{
    public string Nombre;
    public string Apodo;
    public int Level;
    public int Exp;
    public int oro;
    public int Fuerza;
    public int Destreza;
    public int Sabiduria;
    public int Constitucion;
    public int Inteligencia;
    public int Carisma;
    public string idDiscord;
    public string usernameDiscord;
    public string discriminator;


    public static DatosDisc CreateFromJSON(string jsonString)
    {
        try
        {
            Debug.LogError(jsonString + "Response");
            return JsonUtility.FromJson<DatosDisc>(jsonString);
        }
        catch (System.Exception)
        {
            Debug.LogError(jsonString + "Response");


            return JsonUtility.FromJson<DatosDisc>("{}");
        }

         
    }
}