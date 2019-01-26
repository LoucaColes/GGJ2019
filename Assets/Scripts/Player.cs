using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    //Tweaking values
    [SerializeField] private float acceleration = 0f;

    //Player Data
    private Rigidbody2D rg2D;
    private string playerID;
    
    //Movement
    private Vector2 XYmovement = new Vector2(0f, 0f);
    [SerializeField]
    private Vector2 Deadzone = new Vector2(-0.125f, 0.125f);

    //Face Buttons
    private bool XButton = false;
    private bool OButton = false;

    //Rotation
    private Vector2 aimInput;
    private float aimRot;
    private float lastAimRot;

    #endregion

    #region Unity Events
    private void Awake()
    {
        rg2D = GetComponent<Rigidbody2D>();
        InitInputs();
    }

    private void Update()
    {
        GetInputs();
        ApplyInputs();
    }

    /// <summary>
    /// Gets input for all actions
    /// </summary>
    private void GetInputs()
    {

        //Get Movement from controller axis
        XYmovement = new Vector2(Input.GetAxisRaw(playerID + "Horizontal"), -(Input.GetAxisRaw(playerID + "Vertical")));
        
        //Apply Deadzone
        if (XYmovement.x > Deadzone.x && XYmovement.x < Deadzone.y)
        {
            XYmovement.x = 0f;
        }
        if (XYmovement.y > Deadzone.x && XYmovement.y < Deadzone.y)
        {
            XYmovement.y = 0f;
        }

        //Listen for face buttons
        if (Input.GetButtonDown(playerID + "Action"))
        {
            XButton = true;
        }
        if (Input.GetButtonDown(playerID + "Action1"))
        {
            OButton = true;
        }

        //Apply rotation
        aimInput = new Vector2(XYmovement.x, XYmovement.y);
        aimRot = Mathf.Rad2Deg * Mathf.Atan2(aimInput.x, aimInput.y);

        if (aimInput.x == 0 && aimInput.y == 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -lastAimRot));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -aimRot));
            lastAimRot = aimRot;
        }
    }

    /// <summary>
    /// Applies inputs to player character
    /// </summary>
    private void ApplyInputs()
    {
        //Apply movement
        XYmovement *= new Vector2(acceleration, acceleration);
        XYmovement += new Vector2(transform.position.x, transform.position.y);
        rg2D.MovePosition(XYmovement);

        //Add rotation later for object in player hand

        //Action Button
        if (XButton)
        {
            Debug.Log(playerID + " does an X action!");
            XButton = false;
        }
        if (OButton)
        {
            Debug.Log(playerID + " does an O action!");
            OButton = false;
        }
    }

    /// <summary>
    /// Initalise the input mangager values
    /// </summary>
    private void InitInputs()
    {
        switch (gameObject.tag)
        {
            case "Player1":
                playerID = "P1";
                break;
            case "Player2":
                playerID = "P2";
                break;
            case "Player3":
                playerID = "P3";
                break;
            case "Player4":
                playerID = "P4";
                break;
            default:
                Debug.Log("NO CONTROLLER FOR " + gameObject.tag);
                break;
        }
    }
    #endregion
}
