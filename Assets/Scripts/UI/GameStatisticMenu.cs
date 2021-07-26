using UnityEngine;
using UnityEngine.UI;

public class GameStatisticMenu : MonoBehaviour
{
    [SerializeField] private Slider keySlider;

    private void OnEnable()
    {
        ServiceLocator.Subscribe<GameStatisticMenu>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<GameStatisticMenu>();
    }

    public void UpdateKeySlider(float currentValue)
    {
        keySlider.value = currentValue;
    }
}
