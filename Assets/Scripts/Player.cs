using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    //Tweaking values
    [SerializeField] private float acceleration = 0f;
    [SerializeField] private int money = 100;

    //Player Data
    private Rigidbody2D rg2D;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D feetCollider2D;

    [SerializeField] private Collider2D bodyCollider2D;

    [SerializeField] private GameObject shadow;

    [SerializeField] private GameObject blockPref;

    private Vector3 spawnPosition;
    private GameObject tempGameObject;
    private bool actionMode = true;

    private string playerID;
    private int intId;

    public int IntId
    {
        get { return intId; }
    }

    private PlayerManager playerManager;

    [SerializeField] private int startHealth;
    private int health;

    private bool Alive()
    {
        return health <= 0;
    }

    [SerializeField] private float respawnTime;

    //Movement
    private Vector2 XYmovement = new Vector2(0f, 0f);
    private Vector2 XYRightmovement = new Vector2(0f, 0f);
    private Vector2 directionVect = new Vector2(0f, 0f);
    private Vector2 lastDirection = new Vector2(0f, 0f);
    [SerializeField]
    private Vector2 Deadzone = new Vector2(-0.125f, 0.125f);

    private bool allowInput;

    //Input
    public bool AllowInput
    {
        set { allowInput = value; }
    }

    //Face Buttons
    private bool XButton = false;
    private bool OButton = false;
    private bool R1Button = false;

    //Rotation
    private Vector2 aimInput;
    private float aimRot;
    private float lastAimRot;

    //Campfire
    [SerializeField] private Campfire campfire;
    public Campfire Campfire { get{ return campfire; } }

    [SerializeField] private float placeDistance;
    [SerializeField] private float attackDistance;

    [SerializeField]
    private CharacterAnimator characterAnimator;

    public CharacterAnimator CharacterAnimator
    {
        get { return characterAnimator; }
    }

    #endregion

    #region Unity Events
    private void Awake()
    {
        rg2D = GetComponent<Rigidbody2D>();
        InitInputs();
        health = startHealth;
    }

    private void Update()
    {
        if (allowInput)
        {
            GetInputs();
            ApplyInputs();
        }
    }
    #endregion

    public void Init(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
    }

    /// <summary>
    /// Gets input for all actions
    /// </summary>
    private void GetInputs()
    {

        //Get Movement from controller axis
        XYmovement = new Vector2(Input.GetAxisRaw(playerID + "Horizontal"), -(Input.GetAxisRaw(playerID + "Vertical")));
        XYRightmovement = new Vector2(Input.GetAxisRaw(playerID + "RightHorizontal"), -(Input.GetAxisRaw(playerID + "RightVertical")));
        
        //Apply Deadzone
        if (XYmovement.x > Deadzone.x && XYmovement.x < Deadzone.y)
        {
            XYmovement.x = 0f;
        }
        if (XYmovement.y > Deadzone.x && XYmovement.y < Deadzone.y)
        {
            XYmovement.y = 0f;
        }
        if (XYRightmovement.x > Deadzone.x && XYRightmovement.x < Deadzone.y)
        {
            XYRightmovement.x = 0f;
        }
        if (XYRightmovement.y > Deadzone.x && XYRightmovement.y < Deadzone.y)
        {
            XYRightmovement.y = 0f;
        }



        directionVect = XYmovement;

        if (Math.Abs(directionVect.x) > 0.1f)
        {
            lastDirection = directionVect;
        }

        if (Math.Abs(directionVect.sqrMagnitude) < 0.001f)
        {
            //Debug.Log("Stop");
            if (characterAnimator != null)
            {
                characterAnimator.Stop();
            }
        }
        else if (actionMode)
        {
            characterAnimator.Walk();
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
        if (Input.GetButtonDown(playerID + "Action2"))
        {
            R1Button = true;
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

        if (actionMode)
        {
            rg2D.MovePosition(XYmovement);
        }
        //Add rotation later for object in player hand

        //Action Button
        if (XButton)
        {
            Debug.Log(playerID + " does an X action!");
            PlaceDownBlock();
            XButton = false;
        }
        if (OButton && actionMode)
        {
            Debug.Log(playerID + " does an O action!");
            characterAnimator.Stab();
            Attack();
            OButton = false;
        }
        if (R1Button)
        {
            Debug.Log(playerID + " does an R1 action!");
            PlaceBlock();
            R1Button = false;
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
                intId = 0;
                break;
            case "Player2":
                playerID = "P2";
                intId = 1;
                break;
            case "Player3":
                playerID = "P3";
                intId = 2;
                break;
            case "Player4":
                playerID = "P4";
                intId = 3;
                break;
            default:
                Debug.Log("NO CONTROLLER FOR " + gameObject.tag);
                break;
        }
    }

    private void Attack()
    {
        Debug.Log("last movement: " + lastDirection);
        Vector2 normalized = lastDirection.normalized;
        Vector2 xNorm = new Vector2(normalized.x, 0);
        Debug.DrawRay(transform.position, xNorm * attackDistance, Color.magenta, 1f);
        BoxCastUtility.TryDamageAtPosition(transform.position, xNorm * attackDistance, transform.root.tag);
    }

    private void PlaceBlock()
    {
        if (SpendMoney(10) && actionMode)
        {
            Vector2 normalized = XYRightmovement;
            Debug.DrawRay(feetCollider2D.transform.position, normalized * placeDistance, Color.cyan, 1f);
            BoxCastUtility.TrySnapToPosition(feetCollider2D.transform.position, normalized * placeDistance, out spawnPosition);
            tempGameObject = (GameObject)Instantiate(blockPref, spawnPosition, Quaternion.identity);
            tempGameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            actionMode = false;
        }
        else if(!actionMode)
        {
            Destroy(tempGameObject);
            actionMode = true;
        }
    }

    private void PlaceDownBlock()
    {
        tempGameObject.GetComponent<SpriteRenderer>().color = Color.white;
        actionMode = true;
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        health -= 1;
        Debug.Log("Can Respawn: " + campfire.CanRespawn());
        if (health == 0 && campfire.CanRespawn())
        {
            StartCoroutine(Respawn());
        }
        else
        {
            playerManager.SetDeadPlayer(intId, this);
            spriteRenderer.enabled = false;
            feetCollider2D.enabled = false;
            bodyCollider2D.enabled = false;
            shadow.SetActive(false);
        }
    }

    private IEnumerator Respawn()
    {
        //Disable input renderer and collider
        spriteRenderer.enabled = false;
        feetCollider2D.enabled = false;
        bodyCollider2D.enabled = false;
        shadow.SetActive(false);
        allowInput = false;

        //move to campfire
        rg2D.MovePosition(campfire.transform.position);

        //wait for respawn time
        yield return new WaitForSeconds(respawnTime);
        //enable input renderer and collider
        spriteRenderer.enabled = true;
        feetCollider2D.enabled = true;
        bodyCollider2D.enabled = true;
        shadow.SetActive(true);
        health = startHealth;
        allowInput = true;
    }

    public bool SpendMoney(int _money)
    {
        if (money > 0)
        {
            money -= _money;
            if (money <= 0)
            {
                money += _money;
                Debug.Log("Can't spend any more money");
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    public void GainMoney(int _money)
    {
        money += _money;
    }
}
