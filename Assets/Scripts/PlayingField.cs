using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class PlayingField : MonoBehaviour
{
    public bool isStatic = false;

    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Playing field")]
    public int playingWidth = 1;
    [Min(1)]
    public int playingHeight = 1;
    [Min(0)]
    public int xOffset = 0;
    [Min(0)]
    public int yOffset = 0;

    [Header("Ship")]
    public Ship ship;
    public int shipX = 2;
    public int shipY = 2;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera endCamera;
    
    
    [Header("State")]
    public bool isLastLevel = false;
    public State state = State.Prepare;
    public enum State { Prepare, Action, Finish };
    public Tile destination = null;
    public int completed = 0;
    public List<Tile> moveSet;
    public Coroutine stateCoroutine = null;

    [Header("State Durations (Action/Finish Duration needs to be equal or larger than Shift Duration and Move Duration)")]
    public float prepareDuration = 1.0f;
    public float actionDuration = 1.0f;
    public float finishDuration = 1.0f;

    [Header("Animations")]
    public float shiftDuration = 1.0f;
    public float moveDuration = 1.0f;

    public Stopwatch stopwatch = new Stopwatch();

    private GameController gameController;

    public EndTalk endDialog;
    public int beforeEndTrigger = 3;
    private bool endPlayed = false;

    private CanvasGroup canvasGroup;
    private TextMeshProUGUI textfield;

    public float transitionTime = 3;

    [SerializeField] private AudioSource clickOnTile;

    public static Action OnEndingEvent;
    
    public AudioSource[] sailSounds;

    public MoveSetDisplayer moveSetDisplayer;

    private void OnDrawGizmos()
    {
        if (tilemap == null)
            return;
        Gizmos.color = Color.green;

        var o = new Vector3(xOffset, 0, yOffset);
     
        Gizmos.DrawLine(o + new Vector3(0, 0.125f, 0), o + new Vector3(0, 0.125f, playingHeight));
        Gizmos.DrawLine(o + new Vector3(playingWidth, 0.125f, 0), o + new Vector3(playingWidth, 0.125f, playingHeight));
        Gizmos.DrawLine(o + new Vector3(0, 0.125f, 0), o + new Vector3(playingWidth, 0.125f, 0));
        Gizmos.DrawLine(o + new Vector3(0, 0.125f, playingHeight), o + new Vector3(playingWidth, 0.125f, playingHeight));

        foreach (var m in moveSet)
            Gizmos.DrawSphere(new Vector3(m.x + 0.5f, 0, m.y + 0.5f - completed), 0.25f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.instance == null)
        {
            var gameControllerPrefab = (GameObject) Resources.Load("Game Manager");
            var instance = Instantiate(gameControllerPrefab);
            instance.name = "Game Manager";         
            gameController = GameController.instance.GetComponent<GameController>();
        }

        canvasGroup = GameController.instance.uIContoller.transform.GetChild(2).GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        textfield = canvasGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        DrawPlayingFieldBorder();

        if (tilemap)
        {
            tilemap.transform.position = Vector3.zero;
        }

        foreach (var tile in tilemap.map)
        {
            tile.gameObject.SetActive(false);
        }

        for (int x = 0; x < playingWidth; x++)
        {
            for (int y = 0; y < playingHeight + 1; y++)
            {
                var tile = TileIn(x, y);
                if (tile)
                {
                    tile.gameObject.SetActive(true);
                }
            }
        }


        if (ship)
        {
            Move(shipX, shipY, false);
        }

        if (actionDuration < Mathf.Max(shiftDuration, moveDuration))
            actionDuration = Mathf.Max(shiftDuration, moveDuration);

        if (finishDuration < Mathf.Max(shiftDuration, moveDuration))
            finishDuration = Mathf.Max(shiftDuration, moveDuration);
    }


    private void OnEnable() {
        ActiveCharacterEventManager.OnCharacterClicked += ActiveCharacterClicked;
    }

    private void ActiveCharacterClicked(ESkill skill) {
        if (skill == ESkill.GET_HEALTH) {
            var moves = UpdateMoveSet();
            moveSetDisplayer.Show(moves);
        }
    }
    private void DrawPlayingFieldBorder()
    {
        var lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 6;
 
        ColorUtility.TryParseHtmlString("#ffb12e", out Color color);
        lineRenderer.material.color = color;

        var offset = new Vector3(xOffset, 0, yOffset);
        lineRenderer.SetPositions(new Vector3[]
            {
                new Vector3(0, 0.1f, 0) + offset,
                new Vector3(playingWidth, 0.1f, 0) + offset,
                new Vector3(playingWidth, 0.1f, playingHeight) + offset,
                new Vector3(0, 0.1f, playingHeight) + offset,
                new Vector3(0, 0.1f, 0) + offset,
                new Vector3(playingWidth, 0.1f, 0) + offset,
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic) { return; }

        if (stateCoroutine != null)
            return;

        switch (state)
        {
            case State.Prepare: stateCoroutine = StartCoroutine(Prepare()); break;
            case State.Action: stateCoroutine = StartCoroutine(Action()); break;
            case State.Finish: stateCoroutine = StartCoroutine(Finish()); break;
            default: Debug.LogError("Non existing state"); break;
        }
    }

    private IEnumerator Prepare()
    {
        if (PlayToTileCoords(0, shipY).y >= tilemap.end - beforeEndTrigger && endDialog != null && !endPlayed)
        {
            endPlayed = true;
            Debug.Log("End talk");
            endDialog.StartTalk();
        }

        stopwatch.Restart();
        destination = null;

        var moves = UpdateMoveSet();
        moveSetDisplayer.gameObject.SetActive(true);
        moveSetDisplayer.SetPosition(ship.transform.position.x, ship.transform.position.z);
        moveSetDisplayer.Show(moves);


        

        while ((destination = GetDestination()) == null)
        {
            yield return null;
        }
        

        // Click on a tile that is not in the move set
        while (!moveSet.Contains(destination))
        {
            destination = null;
            StopCoroutine(stateCoroutine);
            stateCoroutine = null;
            yield return null;
        }

        // Add food when resting
        if (destination == TileIn(shipX, shipY))
        {
            ship.AddFood(2);
            Debug.Log("Resting");
        }

        yield return new WaitForSeconds(prepareDuration);

        state = State.Action;
        stateCoroutine = null;
        Debug.Log("Prepare over " + stopwatch.ElapsedMilliseconds / 1000);
    }

    private IEnumerator Action()
    {
        moveSetDisplayer.gameObject.SetActive(false);
        moveSetDisplayer.Hide();
        var coord = TileToPlayCoords(destination.x, destination.y);

        if (ship.activeSkills[(int)ESkill.GET_HEALTH]) {
            ship.cooldownCounter = 0;
        }
        
        Move(coord.x, coord.y);

        yield return new WaitForSeconds(actionDuration);

        var tile = TileIn(shipX, shipY);
        
        while (IsInPlayingField(shipX, shipY) && IsInTilemap(shipX, shipY) && !ship.NoFood())
        {
            if (IsShipAtTheEnd())
                break;

            bool again = tile.ApplyEffect(this, out bool wait);

            if (wait)
                yield return new WaitForSeconds(actionDuration);

            if (!again)
                break;

            tile = TileIn(shipX, shipY);

            if (tile == null)
                break;
        }

        state = State.Finish;
        stateCoroutine = null;
        Debug.Log("Action over " + stopwatch.ElapsedMilliseconds / 1000);
    }

    private bool IsShipAtTheEnd()
    {
        return PlayToTileCoords(shipX, shipY).y == tilemap.end - 1;
    }

    private IEnumerator WaitAndSetFade() {
        yield return new WaitForSeconds(1.5f);
        GameController.instance.NextScene();
    }
    
    private IEnumerator Finish()
    {
        ship.ConsumeFood();
        Shift();

        yield return new WaitForSeconds(finishDuration);

        if (IsShipAtTheEnd() && isLastLevel) {
            OnEndingEvent?.Invoke();
            StartCoroutine(WaitAndSetFade());
            
        } else if (IsShipAtTheEnd())
        {
            Debug.Log("End");
            GameController.instance.NextScene();
        }

      // ondra

        if (IsGameOver())
        {
            if (ship.activeSkills[(int)ESkill.DEATH_SKIP]) {
                Move(playingWidth/2, playingHeight/2, false);
                ship.activeSkills[(int)ESkill.DEATH_SKIP] = false;
                Ship.OnShipRessurected?.Invoke();
            }
            else {
                Debug.Log("End");

                var text = !IsInPlayingField(shipX, shipY) ? "Out" : "Hungry";
                textfield.text = text;

                canvasGroup.DOFade(1f, 2f).OnComplete(() =>
                {
                    textfield.gameObject.SetActive(true);
                });

                yield return new WaitForSeconds(3f);

                textfield.gameObject.SetActive(false);

                yield return new WaitForSeconds(1);
                canvasGroup.alpha = 0;

                GameController.instance.Restart();
            }
        }

        state = State.Prepare;
        stateCoroutine = null;
        Debug.Log("Finish over " + stopwatch.ElapsedMilliseconds / 1000);
    }

    public bool IsGameOver()
    {
        return !IsInPlayingField(shipX, shipY) || ship.NoFood();
    }

    public Tile GetDestination()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return null;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Tile")))
            {
                clickOnTile.Play();
                
                var spot = hit.transform.gameObject.GetComponent<MoveTile>();

                if (spot == null || spot.blocked)
                    return null;

                var tileObject = TileIn(shipX + spot.direction.x, shipY + spot.direction.y);

                var tile = tileObject.GetComponent<Tile>();

                if (tile != null)
                {
                    return tile;
                }

                if (shipY == playingHeight - 1 && hit.transform.gameObject.GetComponent<Ship>() != null)
                {
                    return TileIn(shipX, shipY);
                }
            }
        }


        return null;
    }

    public void Shift()
    {
        var drop = 5;
        Debug.Log("Shift " + tilemap.transform.localPosition + " " + stopwatch.ElapsedMilliseconds / 1000 );
        tilemap.transform.DOLocalMoveZ(tilemap.transform.localPosition.z - 1, shiftDuration);
        Move(shipX, shipY - 1);
        for (int x = 0; x < tilemap.width; x++)
        {        
            var tile = TileIn(x, 0);
            tile.transform.DOLocalMoveY(-drop, shiftDuration).OnComplete(() => tile.gameObject.SetActive(false));
        }
        for (int x = 0; x < tilemap.width; x++)
        {
            var tile = TileIn(x, playingHeight + 1);
            
            if (tile == null)
                continue;

            tile.gameObject.SetActive(true);
            tile.transform.DOLocalMoveY(0, shiftDuration).From(drop);
        }
        completed++;
    }

    public void PlaySailSound()
    {
        if (sailSounds.Length > 0)
        {
            int randomSourceIndex = UnityEngine.Random.Range(0, sailSounds.Length);
            AudioSource selectedSource = sailSounds[randomSourceIndex];
            selectedSource.Play();
        }
    }

    void Move(int x, int y, bool tween = true)
    {
        Debug.Log("Move : " + x + " " + y + " " + stopwatch.ElapsedMilliseconds / 1000 );

        if (tween)
        {
            //ship.transform.DOMove(new Vector3(x + 0.5f + xOffset, 0, y + 0.5f + yOffset), moveDuration);
            ship.transform.DOMoveX(x + 0.5f + xOffset, moveDuration);
            ship.transform.DOMoveZ(y + 0.5f + yOffset, moveDuration);

            if (y >= shipY)
            {
                var forward = new Vector2(0, 1);
                var dest = new Vector2(x - shipX, y - shipY);
                var angle = -Vector2.SignedAngle(forward, dest);


                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => PlaySailSound());
                sequence.Append(ship.transform.DORotate(new Vector3(0, angle, 0), moveDuration / 2).SetEase(Ease.OutSine));
                sequence.Append(ship.transform.DORotate(Vector3.zero, moveDuration / 2).SetEase(Ease.InSine));
            }
        }
        else
            ship.transform.position = new Vector3(x + 0.5f + xOffset, 0, y + 0.5f + yOffset);

        shipX = x;
        shipY = y;
    }

    public void Move(Direction direction)
    {
        var dir = DirTranslation.DirToVec[(int)direction];
        Move(shipX + dir.x, shipY + dir.y);
    }

    public bool[] UpdateMoveSet()
    {
        List<Tile> result = new List<Tile>();

        bool[] moveBools = new bool[(int)Direction.None + 1];

        for (Direction dir = 0; dir < Direction.None; dir++) 
        {
            var t = DirTranslation.DirToVec[(int)dir];
            var x = shipX + t.x;
            var y = shipY + t.y;

            if (!IsInPlayingField(x, y) || !IsInTilemap(x, y))
            {
                moveBools[(int)dir] = false;
                continue;
            }

            var tile = TileIn(x, y);

            if (!tile.IsMoveable(ship))
            {
                moveBools[(int)dir] = false;
                continue;
            }

            moveBools[(int)dir] = true;
            result.Add(tile);            
        }

        if (shipY == playingHeight - 1 && IsInTilemap(shipX, shipY))
        {
            moveBools[(int)Direction.None] = true; 
            result.Add(TileIn(shipX, shipY));
        }
        else
            moveBools[(int)Direction.None] = false;

        moveSet = result;
        return moveBools;
    }

    private Tile TileIn(int x, int y)
    {
        var tileCoord = PlayToTileCoords(x, y);

        if (tileCoord.x < 0 || tilemap.width  <= tileCoord.x ||
            tileCoord.y < 0 || tilemap.height <= tileCoord.y)
            return null;

        return tilemap.At(tileCoord.x, tileCoord.y);
    }

    public Vector2Int PlayToTileCoords(int x, int y)
    {
        return new Vector2Int(x + xOffset, y + yOffset + completed);
    }

    private Vector2Int TileToPlayCoords(int x, int y)
    {
        return new Vector2Int(x - xOffset, y - yOffset - completed); 
    }

    public bool IsInPlayingField(int x, int y)
    {
        return 0 <= x && x < playingWidth &&
               0 <= y && y < playingHeight;
    }

    public bool IsInTilemap(int x, int y)
    {
        var coord = PlayToTileCoords(x, y);
        return 0 <= coord.x && coord.x < tilemap.width &&
               0 <= coord.y && coord.y < tilemap.height;
    }

    TileTypeShort TileTypeIn(int x, int y)
    {
        return TileIn(x, y).tileType;
    }
}
