using UnityEngine;

[CreateAssetMenu(fileName = "New trap", menuName = "Trap configuration")]
public class TrapConfiguration : ScriptableObject
{
   [SerializeField] private GameObject trapGameObject;
   [SerializeField] private TrapType trapType;
   [SerializeField] private int startCountInFactory;

   public GameObject TrapGameObject => trapGameObject;
   public TrapType TrapType => trapType;
   public int StartCountInFactory => startCountInFactory;
}
