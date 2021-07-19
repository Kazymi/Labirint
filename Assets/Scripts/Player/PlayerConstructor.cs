using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerConstructor : MonoBehaviour
{
      [SerializeField] private Movenment movenment;
      [SerializeField] private GameObject camera;
      [SerializeField] private AnimationControl animationControl;
      
      private InputHandler _inputHandler;
      private PhotonView _pv;

      private void Start()
      {
            _pv = GetComponent<PhotonView>();
            if (_pv.IsMine)
            {
                  _inputHandler = ServiceLocator.GetService<InputHandler>();
                  movenment.Initialize(_inputHandler);
                  animationControl.Initialized(_inputHandler);
            }
            else
            {
                  Destroy(movenment);
                  Destroy(camera);
                  Destroy(animationControl);
            }
      }
}
