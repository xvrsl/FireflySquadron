using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [SerializeField]
    public GameSettings gameSettings = new GameSettings();

    [Header("Game Status")]
    public GamePhase ROGamePhase;
    public int turnCounter;
    public enum GamePhase
    {
        plan = 0,
        execute = 1
    }
    GamePhase _gamePhase = GamePhase.plan;
    public GamePhase gamePhase
    {
        get
        {
            return _gamePhase;
        }
        set
        {
            if(value != _gamePhase)
            {
                _gamePhase = value;
                if(value == GamePhase.plan)
                {
                    PlanPhaseStart();
                }
                else
                {
                    ExecutePhaseStart();

                }
            }
        }
    }
    public bool isExecutePhase
    {
        get
        {
            if(gamePhase == GamePhase.execute)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool isPlanPhase
    {
        get
        {
            return !isExecutePhase;
        }
    }
    public float executePhaseTimer = 0f;
    public float t
    {
        get
        {
            if(gameSettings.executeTime <= 0)
            {
                return 0;
            }
            else
                return executePhaseTimer / gameSettings.executeTime;
        }
    }

    public delegate void GameManagerEventHandler();
    public event GameManagerEventHandler onPlanPhaseStart;
    public event GameManagerEventHandler onExecutePhaseStart;
    void PlanPhaseStart()
    {
        turnCounter++;
        if(onPlanPhaseStart != null)
            onPlanPhaseStart.Invoke();
    }
    void ExecutePhaseStart()
    {
        executePhaseTimer = 0;
        if(onExecutePhaseStart != null)
            onExecutePhaseStart.Invoke();
    }

    private void Update()
    {
        ROGamePhase = gamePhase;
    }

    private void FixedUpdate()
    {
        if (gamePhase == GamePhase.execute)
        {
            executePhaseTimer += Time.fixedDeltaTime;
            if (executePhaseTimer >= gameSettings.executeTime)
            {
                TogglePlanExecutePhase();
            }
        }
    }

    public void RegisterPlanExecuteBehaviour(PlanExecuteBehaviour planExecuteBehaviour)
    {
        onPlanPhaseStart += planExecuteBehaviour.OnPlanPhaseStart;
        onExecutePhaseStart += planExecuteBehaviour.OnExecutePhaseStart;
    }
    public void LogoutPlanExecuteBehaviour(PlanExecuteBehaviour planExecuteBehaviour)
    {
        onPlanPhaseStart -= planExecuteBehaviour.OnPlanPhaseStart;
        onExecutePhaseStart -= planExecuteBehaviour.OnExecutePhaseStart;
    }
    void Initialize()
    {
        gamePhase = GamePhase.plan;
        if (onPlanPhaseStart != null)
        {
            onPlanPhaseStart.Invoke();
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Initialize();
    }

    public void TogglePlanExecutePhase()
    {
        if(gamePhase == GamePhase.execute)
        {
            gamePhase = GamePhase.plan;
        }
        else
        {
            gamePhase = GamePhase.execute;
        }
    }

    [System.Serializable]
    public class GameSettings
    {
        public float executeTime = 5f;
        public bool friendlyDamage = true;//Not implemented yet
        public bool othersShipEngineOperationVisible = false;
    }
}
