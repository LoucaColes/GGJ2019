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
        
        //Apply Deadzone
        if (XYmovement.x > Deadzone.x && XYmovement.x < Deadzone.y)
        {
            XYmovement.x = 0f;
        }
        if (XYmovement.y > Deadzone.x && XYmovement.y < Deadzone.y)
        {
            XYmovement.y = 0f;
        }

        directionVect = XYmovement;

        if (Math.Abs(directionVect.x) > 0.1f)
        {
            lastDirection = directionVect;
        }

        if (Math.Abs(directionVect.sqrMagnitude) < 0.001f)
        {
            Debug.Log("Stop");
            if (characterAnimator != null)
            {
                characterAnimator.Stop();
            }
        }

        if (characterAnimator != null)
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
            PlaceBlock();
            XButton = false;
        }
        if (OButton)
        {
            Debug.Log(playerID + " does an O action!");
            characterAnimator.Stab();
            Attack();
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
        Vector2 normalized = lastDirection.normalized;
        Debug.DrawRay(feetCollider2D.transform.position, normalized * placeDistance, Color.cyan, 1f);
        Vector3 spawnPosition;
        BoxCastUtility.TrySnapToPosition(feetCollider2D.transform.position, normalized * placeDistance, out spawnPosition);
        GameObject tempGameObject = (GameObject) Instantiate(blockPref, spawnPosition, Quaternion.identity);
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

    public void SpendMoney(int _money)
    {
        money -= _money;
    }
    public void GainMoney(int _money)
    {
        money += _money;
    }
}
