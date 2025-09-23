using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/* CHANGES for TREASURE PROJECT:
 * Can goto next or previous scene
 * Add GetUIObject, as GetObject didn't always work
 * Can exit the game
 * Can goto the last current scene
 */

public class ScnManager : MonoBehaviour
{
    /** PRIVATE STATICS **/

    static string[] SCN_MANAGER_REQUIREDOBJS = {
        "ScnPrior",
        "ScnPrior/ScnPriorCamera",
        "ScnPrior/ScnPriorUI",
        "ScnPrior/ScnPriorUI/ScnPriorUIBoundary",
        "ScnPrior/ScnPriorUI/ScnPriorUIEvents",
        "ScnMain",
    };

    enum SCNType
    {
        UNITY_ROOT,
        PRIOR,
        UI,
        MAIN
    };

    const string SCN_MANAGER_INSTANCENAME = "ScnMain";
    const bool Debugm = true;

    Vector2 cameraPosition;
    Vector2 cameraShake;

    float cameraShakeLevel;

    public GameObject FollowObject;

    static int lastScene = -1;

    static GameObject getScnObj_(string scnObj)
    {
        GameObject found = null;

        string[] hierarchy = scnObj.Split('/');

        string total = "/";
        foreach (string entry in hierarchy)
        {
            total += entry + "/";
            found = GameObject.Find(total);
            Debug.Assert(found != null, "ScnObj " + total + " doesnt exist. Is this a BBScn?");
        }

        //Debug.Log("got obj '" + total + "' Return.");

        return found;
    }
    static void checkScnObj_(string scnObj)
    {
        GameObject dummy = getScnObj_(scnObj);
    }
    static void init_()
    {
        // Check if the required GameObjects are there.
        foreach(string obj in SCN_MANAGER_REQUIREDOBJS)
        {
            checkScnObj_(obj);
        }
    }

    /** PUBLIC STATICS **/

    /// <summary>
    /// Returns the Scene Manager Instance.
    /// </summary>
    /// <returns>Scene Manager Instance</returns>
    public static ScnManager Instance()
    {
        return getScnObj_(SCN_MANAGER_INSTANCENAME).GetComponent<ScnManager>();
    }

    /// <summary>
    /// Retrieves a GameObject.
    /// </summary>
    /// <param name="obj">GameObject Name.</param>
    /// <returns>Found GameObject</returns>
    public static GameObject GetObject(string obj)
    {
        return getScnObj_(obj);
    }

    /// <summary>
    /// Retrieves a GameObject from ScnPrior/ScnPriorUI/ScnPriorUIBoundary.
    /// </summary>
    /// <param name="obj">GameObject Name.</param>
    /// <returns>Found GameObject</returns>
    public static GameObject GetUIObject(string obj)
    {
        return getScnObj_("ScnPrior/ScnPriorUI/ScnPriorUIBoundary/" + obj);
    }

    /// <summary>
    /// Returns the Scene Camera.
    /// </summary>
    /// <returns>Scene Camera</returns>
    public static Camera GetCamera()
    {
        return GetObject("ScnPrior/ScnPriorCamera").GetComponent<Camera>();
    }

    /// <summary>
    /// Goto to the next scene via build index
    /// </summary>
    public static void Next()
    {
        Goto(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// Goto to the previous scene via build index
    /// </summary>
    public static void Previous()
    {
        Goto(SceneManager.GetActiveScene().buildIndex - 1);
    }
    /// <summary>
    /// Goto to a new scene via name
    /// </summary>
    /// <param name="name">Scene Name.</param>
    public static void Goto(string name)
    {
        Debug.Assert(lastScene != -1, "can't do last.");
        lastScene = SceneManager.GetActiveScene().buildIndex;
        SCENEManager.ChangeScene(name);
    }
    /// <summary>
    /// Goto to a new scene via ID
    /// </summary>
    /// <param name="name">Scene ID.</param>
    public static void Goto(int id)
    {
        Debug.Assert(lastScene != -1, "can't do last.");
        lastScene = SceneManager.GetActiveScene().buildIndex;
        SCENEManager.ChangeScene(id);
    }
    /// <summary>
    /// Goto to the last scene via build index
    /// </summary>
    public static void Last()
    {
        int old = lastScene;
        Goto(old);
    }
    /// <summary>
    /// Restart current scene
    /// </summary>
    public static void Reload()
    {
        SCENEManager.Restart();
    }
    /// <summary>
    /// Exit the game
    /// </summary>
    public static void Exit()
    {
        SCENEManager.ExitGame();
    }

    public void SetCameraPosition(Vector2 vector)
    {
        cameraPosition = vector;
    }

    public void SetCameraPosition(float x, float y)
    {
        SetCameraPosition(new Vector2(x, y));
    }

    public void SetCameraShakeLevel(float level)
    {
        cameraShakeLevel = level;
    }

    System.Random rngGen;

    /** MONOBEHAVIOUR OVERRIDES **/
    private void Start()
    {
        Debug.Assert(gameObject.name == SCN_MANAGER_INSTANCENAME, "Must call it from " + SCN_MANAGER_INSTANCENAME);
        init_();
        cameraPosition = GetCamera().transform.position;
        cameraShake = new Vector2(0, 0);
        cameraShakeLevel = 0;

        rngGen = new System.Random();

        if (GameObject.Find("ScnPointStart") != null)
        {
            Debug.Log("ScnPointStart");
        }
    }
    Vector3 fPos;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SCENEManager.ExitGame();
        }
        if (cameraShakeLevel > 0)
        {
            int shake = (int)(cameraShakeLevel * 10000) / 2;
            cameraShake.x = (float)rngGen.Next(-shake, shake) / 10000;
            cameraShake.y = (float)rngGen.Next(-shake, shake) / 10000;
        }
        GetCamera().transform.position = cameraPosition + cameraShake;

        if (cameraShakeLevel > 0)
        {
            cameraShakeLevel -= Time.deltaTime * 5;
        }
        if (cameraShakeLevel < 0)
        {
            cameraShakeLevel = 0;
        }

        GameObject startXBound = GameObject.Find("ScnXPointStart");
        GameObject endXBound = GameObject.Find("ScnXPointEnd");
        if (FollowObject)
        {
            Vector3 newPos = FollowObject.transform.position;

            if (startXBound || endXBound)
            {
                if (startXBound)
                {
                    if (newPos.x <= startXBound.transform.position.x)
                    {
                        newPos.x = startXBound.transform.position.x;
                    }
                }
                if (endXBound)
                {
                    if (newPos.x >= endXBound.transform.position.x)
                    {
                        newPos.x = endXBound.transform.position.x;
                    }
                }
            }
            else
            {
                newPos.x = cameraPosition.x;
            }

            newPos.y = cameraPosition.y;

            cameraPosition = newPos;
        }
    }
}
