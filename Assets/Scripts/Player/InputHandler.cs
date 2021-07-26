using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick playerJoystick;
    [SerializeField] private bool mobile;

    private PlayerTrigger _playerTrigger;
    private PlayerPunch _playerPunch;
    private Movenment _movement;
    private bool _paused;
    private GameMenu _gameMenu;

    private void OnEnable()
    {
        ServiceLocator.Subscribe<InputHandler>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<InputHandler>();
    }

    private void Start()
    {
        _gameMenu = ServiceLocator.GetService<GameMenu>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // TODO: it's not InputHandler responsibility to activate action. It should raise event and whoever interested can listen
            _playerTrigger.OnClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _playerPunch.Punch();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            _movement.enabled = !_paused;
            _gameMenu.Paused(_paused);
        }
    }

    public void Initialize(PlayerTrigger playerTrigger, PlayerPunch playerPunch, Movenment movenment)
    {
        _playerPunch = playerPunch;
        _playerTrigger = playerTrigger;
        _movement = movenment;
    }

    public Vector2 MoveDirection()
    {
        if (mobile)
        {
            return -playerJoystick.Direction;
        }

        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}