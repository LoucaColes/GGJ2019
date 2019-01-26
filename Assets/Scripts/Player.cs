using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    //Tweaking values
    [SerializeField] private float acceleration = 0f;

    private Rigidbody2D rg2D;
    
    //Movement
    private Vector2 XYmovement = new Vector2(0f, 0f);
    private Vector2 Deadzone = new Vector2(-0.125f, 0.125f);

    //Face Buttons
    private bool actionButton = false;

    #endregion

    #region Unity Events
    private void Awake()
    {
        rg2D = GetComponent<Rigidbody2D>();
        
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
        XYmovement = new Vector2(Input.GetAxisRaw("Horizontal"), -(Input.GetAxisRaw("Vertical")));
        
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
        if (Input.GetButtonDown("Fire1"))
        {
            actionButton = true;
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
        if (actionButton)
        {
            Debug.Log("Player1 does an action!");
            actionButton = false;
        }
    }

    /// <summary>
    /// Initalise the input mangager values
    /// </summary>
    private void InitInputs()
    {

    }
    #endregion
}
